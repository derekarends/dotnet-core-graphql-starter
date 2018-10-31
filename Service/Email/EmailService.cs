using System.Threading.Tasks;
using Core.Email;

namespace Service.Email
{
    public class EmailService : IEmailService
    {
        public async Task SendEmailAsync(string email, string subject, string message)
        {
            await Task.Run(() => {});
        }
    }
}