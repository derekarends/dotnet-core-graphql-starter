using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Core.Settings;
using Core.Token;
using Core.User;
using Microsoft.IdentityModel.Tokens;

namespace Service.Token
{
    public class TokenService : ITokenService
    {
		private readonly DateTime _now = DateTime.UtcNow;
        private readonly SettingsModel _settingsModel;

        public TokenService(SettingsModel settingsModel)
        {
            _settingsModel = settingsModel;
        }

        public string CreateToken(UserModel userModel)
        {
            var claims = new List<Claim> {
                new Claim(JwtRegisteredClaimNames.Sub, userModel.Name),
                new Claim(JwtRegisteredClaimNames.Email, userModel.Email),
                new Claim(JwtRegisteredClaimNames.UniqueName, userModel.Id),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            foreach (var role in userModel.Roles)
            {
                claims.Add(new Claim("role", role));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settingsModel.JwtKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                _settingsModel.JwtIssuer,
                _settingsModel.JwtIssuer,
                claims,
                expires: _now.AddMinutes(_settingsModel.JwtExpiresInMinutes),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public JwtSecurityToken ValidateToken(string token)
        {
            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settingsModel.JwtKey));

            TokenValidationParameters validation = new TokenValidationParameters()
            {
                ValidAudience = _settingsModel.JwtIssuer,
                ValidIssuer = _settingsModel.JwtIssuer,
                ValidateIssuer = true,
                ValidateLifetime = true,
                LifetimeValidator = CustomLifetimeValidator,
                RequireExpirationTime = true,
                IssuerSigningKey = key,
                ValidateIssuerSigningKey = true,
            };

            SecurityToken securityToken;

            ClaimsPrincipal principal = handler.ValidateToken(token, validation, out securityToken);

            return securityToken as JwtSecurityToken;
        }

        private bool CustomLifetimeValidator(DateTime? notBefore, DateTime? expires, SecurityToken tokenToValidate, TokenValidationParameters @param)
        {
            if (expires != null)
            {
                return expires > _now;
            }
            return false;
        }
    }
}
