using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Core.User;
using Prometheus;

namespace Repository.User
{
    public class UserMetrics : IUserRepository
    {
        private const int Increment = 1;
        
        private readonly IUserRepository _repo;
        private readonly ICounter _counter;
        private readonly ISummary _summary;

        public UserMetrics(IUserRepository repo, ICounter counter, ISummary summary)
        {
            _repo = repo;
            _counter = counter;
            _summary = summary;
        }

        public async Task<IEnumerable<UserModel>> FindAsync()
        {
            _counter.Inc(Increment);
            
            var sw = Stopwatch.StartNew();
            sw.Start();
            var r = await _repo.FindAsync();
            sw.Stop();
            
            _summary.Observe(sw.ElapsedMilliseconds);
            
            return r;
        }

        public async Task<UserModel> FindByIdAsync(string id)
        {
            _counter.Inc(Increment);
            
            var sw = Stopwatch.StartNew();
            sw.Start();
            var r = await _repo.FindByIdAsync(id);
            sw.Stop();
            
            _summary.Observe(sw.ElapsedMilliseconds);
            
            return r;
        }

        public async Task<UserModel> FindByEmailAsync(string email)
        {
            _counter.Inc(Increment);
            
            var sw = Stopwatch.StartNew();
            sw.Start();
            var r = await _repo.FindByEmailAsync(email);
            sw.Stop();
            
            _summary.Observe(sw.ElapsedMilliseconds);
            
            return r;
        }

        public async Task<UserModel> InsertAsync(UserModel user)
        {
            _counter.Inc(Increment);
            
            var sw = Stopwatch.StartNew();
            sw.Start();
            var r = await _repo.InsertAsync(user);
            sw.Stop();
            
            _summary.Observe(sw.ElapsedMilliseconds);
            
            return r;
        }

        public async Task<bool> UpdateAsync(UserModel user)
        {
            _counter.Inc(Increment);
            
            var sw = Stopwatch.StartNew();
            sw.Start();
            var r = await _repo.UpdateAsync(user);
            sw.Stop();
            
            _summary.Observe(sw.ElapsedMilliseconds);
            
            return r;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            _counter.Inc(Increment);
            
            var sw = Stopwatch.StartNew();
            sw.Start();
            var r = await _repo.DeleteAsync(id);
            sw.Stop();
            
            _summary.Observe(sw.ElapsedMilliseconds);
            
            return r;
        }
    }
}