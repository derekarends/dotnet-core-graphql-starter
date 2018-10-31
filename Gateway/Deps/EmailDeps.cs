using Core.Email;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Prometheus;
using Service.Email;

namespace Gateway.Deps
{
    public static class EmailDeps
    {
        
        public static void AddEmailDeps(this IServiceCollection services, ILogger logger)
        {
            var sp = services.BuildServiceProvider();
            
            IEmailService es = new EmailService();
            es = new EmailMetrics(es, 
                Metrics.CreateCounter("EmailCounter", "This is the email counter"), 
                Metrics.CreateSummary("EmailSummary", "This is the email summary", new SummaryConfiguration())
            );
            services.AddSingleton(es);
        }
    }
}