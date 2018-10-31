using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using Core.Token;
using Core.User;
using Prometheus;

namespace Service.Token
{
    public class TokenMetrics : ITokenService
    {
        private readonly ITokenService _service;
        private readonly ICounter _counter;
        private readonly ISummary _summary;

        public TokenMetrics(ITokenService service, ICounter counter, ISummary summary)
        {
            _service = service;
            _counter = counter;
            _summary = summary;
        }
        
        public string CreateToken(UserModel userModel)
        {
            _counter.Inc(1);
            
            var sw = Stopwatch.StartNew();
            sw.Start();
            var r = _service.CreateToken(userModel);
            sw.Stop();
            
            _summary.Observe(sw.ElapsedMilliseconds);
            
            return r;
        }

        public JwtSecurityToken ValidateToken(string token)
        {
            _counter.Inc(1);
            
            var sw = Stopwatch.StartNew();
            sw.Start();
            var r = _service.ValidateToken(token);
            sw.Stop();
            
            _summary.Observe(sw.ElapsedMilliseconds);
            
            return r;
        }
    }
}