using System.IdentityModel.Tokens.Jwt;
using Core.User;

namespace Core.Token
{
    public interface ITokenService
    {
        string CreateToken(UserModel userModel);
        JwtSecurityToken ValidateToken(string token);
    }
}