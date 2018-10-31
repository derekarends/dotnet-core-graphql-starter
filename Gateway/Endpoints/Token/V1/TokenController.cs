using System.Threading.Tasks;
using Core.Token;
using Core.User;
using Gateway.Endpoints.Token.V1.Model.Binding;
using Microsoft.AspNetCore.Mvc;

namespace Gateway.Endpoints.Token.V1
{
    [ApiVersion( "1.0" )]
    [Route( "api/v{version:apiVersion}/[controller]" )]
    public class TokenController : Controller
    {
        private readonly ITokenService _tokenService;
        private readonly IUserService _userService;

        public TokenController(ITokenService tokenService, IUserService userService)
        {
            _tokenService = tokenService;
            _userService = userService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateToken([FromBody]LoginModel login)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
			
            var user = await _userService.AuthenticateAsync(login.Email, login.Password);
            if (user == null)
                return Unauthorized();

            var tokenString = _tokenService.CreateToken(user);

            return Ok(new { token = tokenString });
        }
    }
}