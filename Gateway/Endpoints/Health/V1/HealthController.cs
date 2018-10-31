using Microsoft.AspNetCore.Mvc;

namespace Gateway.Endpoints.Health.V1
{
    [ApiVersion( "1.0" )]
    [Route( "api/v{version:apiVersion}/[controller]" )]
    public class HealthController : Controller
    {
        
        [HttpGet]
        public string Get()
        {
            return "Healthy";
        }
    }
}