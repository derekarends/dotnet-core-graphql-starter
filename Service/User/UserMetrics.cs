using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Core.Context;
using Core.User;
using Prometheus;

namespace Service.User
{
    public class UserMetrics : IUserService
    {
        private const int Increment = 1;
        
        private readonly IUserService _service;
        private readonly ICounter _counter;
        private readonly ISummary _summary;

        public UserMetrics(IUserService service, ICounter counter, ISummary summary)
        {
            _service = service;
            _counter = counter;
            _summary = summary;
        }

        public async Task<UserModel> CreateAsync(UserModel user)
        {
            _counter.Inc(Increment);
            
            var sw = Stopwatch.StartNew();
            sw.Start();
            var r = await _service.CreateAsync(user);
            sw.Stop();
            
            _summary.Observe(sw.ElapsedMilliseconds);
            
            return r;
        }

        public async Task<UserModel> AuthenticateAsync(string email, string password)
        {
            _counter.Inc(Increment);
            
            var sw = Stopwatch.StartNew();
            sw.Start();
            var r = await _service.AuthenticateAsync(email, password);
            sw.Stop();
            
            _summary.Observe(sw.ElapsedMilliseconds);
            
            return r;
        }

        public async Task<bool> ChangePasswordAsync(string userId, string oldPassword, string newPassword)
        {
            _counter.Inc(Increment);
            
            var sw = Stopwatch.StartNew();
            sw.Start();
            var r = await _service.ChangePasswordAsync(userId, oldPassword, newPassword);
            sw.Stop();
            
            _summary.Observe(sw.ElapsedMilliseconds);
            
            return r;
        }

        public async Task ForgotPasswordAsync(string email)
        {
            _counter.Inc(Increment);
            
            var sw = Stopwatch.StartNew();
            sw.Start();
            await _service.ForgotPasswordAsync(email);
            sw.Stop();
            
            _summary.Observe(sw.ElapsedMilliseconds);
        }

        public async Task<bool> ResetPasswordAsync(string userId, string newPassword)
        {
            _counter.Inc(Increment);
            
            var sw = Stopwatch.StartNew();
            sw.Start();
            var r = await _service.ResetPasswordAsync(userId, newPassword);
            sw.Stop();
            
            _summary.Observe(sw.ElapsedMilliseconds);
            
            return r;
        }

        public async Task DeleteAsync(string id)
        {
            _counter.Inc(Increment);
            
            var sw = Stopwatch.StartNew();
            sw.Start();
            await _service.DeleteAsync(id);
            sw.Stop();
            
            _summary.Observe(sw.ElapsedMilliseconds);
        }

        public async Task<IEnumerable<UserModel>> GetAsync()
        {
            _counter.Inc(Increment);
                        
            var sw = Stopwatch.StartNew();
            sw.Start();
            var r = await _service.GetAsync();
            sw.Stop();
                        
            _summary.Observe(sw.ElapsedMilliseconds);
            
            return r;
        }

        public async Task<UserModel> GetByIdAsync(ContextModel ctx, string id)
        { 
            _counter.Inc(Increment);
            
            var sw = Stopwatch.StartNew();
            sw.Start();
            var r = await _service.GetByIdAsync(ctx, id);
            sw.Stop();
            
            _summary.Observe(sw.ElapsedMilliseconds);
            
            return r;
        }

        public async Task<UserModel> GetByEmailAsync(string email)
        {
            _counter.Inc(Increment);
            
            var sw = Stopwatch.StartNew();
            sw.Start();
            var r = await _service.GetByEmailAsync(email);
            sw.Stop();
            
            _summary.Observe(sw.ElapsedMilliseconds);
            
            return r;
        }
    }
}