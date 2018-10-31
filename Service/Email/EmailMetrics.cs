using System.Diagnostics;
using System.Threading.Tasks;
using Core.Email;
using Prometheus;

namespace Service.Email
{
    public class EmailMetrics : IEmailService
    {
        private const int Increment = 1;
        
        private readonly IEmailService _service;
        private readonly ICounter _counter;
        private readonly ISummary _summary;

        public EmailMetrics(IEmailService service, ICounter counter, ISummary summary)
        {
            _service = service;
            _counter = counter;
            _summary = summary;
        }
        
        public async Task SendEmailAsync(string email, string subject, string message)
        {
            _counter.Inc(Increment);
            
            var sw = Stopwatch.StartNew();
            sw.Start();
            await _service.SendEmailAsync(email, subject, message);
            sw.Stop();
            
            _summary.Observe(sw.ElapsedMilliseconds);
        }
    }
}