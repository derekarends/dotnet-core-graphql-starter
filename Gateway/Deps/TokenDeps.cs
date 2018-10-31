using Core.Settings;
using Core.Token;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Prometheus;
using Service.Token;

namespace Gateway.Deps
{
    public static class TokenDeps
    {
        
        public static void AddTokenDeps(this IServiceCollection services, ILogger logger)
        {
            var sp = services.BuildServiceProvider();
            
            ITokenService ts = new TokenService(sp.GetService<SettingsModel>());
            ts = new TokenMetrics(ts, 
                Metrics.CreateCounter("TokenCounter", "This is the token counter"), 
                Metrics.CreateSummary("TokenSummary", "This is the token summary", new SummaryConfiguration())
            );
            services.AddSingleton(ts);
        }
    }
}