using System.Collections.Generic;
using System.Threading.Tasks;
using Core.User;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace Repository.User
{
    public class UserCache : IUserRepository
    {
        private readonly IUserRepository _repo;
        private readonly IDistributedCache _cache;

        public UserCache(IUserRepository repo, IDistributedCache cache)
        {
            _repo = repo;
            _cache = cache;
        }

        public async Task<IEnumerable<UserModel>> FindAsync()
        {
            return await _repo.FindAsync();
        }

        public async Task<UserModel> FindByIdAsync(string id)
        {
            var cachedValue = await _cache.GetStringAsync(id);
            if (!string.IsNullOrWhiteSpace(cachedValue))
                return JsonConvert.DeserializeObject<UserModel>(cachedValue);

            var r = await _repo.FindByIdAsync(id);

            await _cache.SetStringAsync(id, JsonConvert.SerializeObject(r));
            return r;
        }

        public async Task<UserModel> FindByEmailAsync(string email)
        {
            return await _repo.FindByEmailAsync(email);
        }

        public async Task<UserModel> InsertAsync(UserModel user)
        {
            var r = await _repo.InsertAsync(user);

            await _cache.SetStringAsync(r.Id, JsonConvert.SerializeObject(r));

            return r;
        }

        public async Task<bool> UpdateAsync(UserModel user)
        {
            var r = await _repo.UpdateAsync(user);
            if (r)
                await _cache.SetStringAsync(user.Id, JsonConvert.SerializeObject(user));

            return r;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var r = await _repo.DeleteAsync(id);
            if (r)
                await _cache.RemoveAsync(id);
            
            return r;
        }
    }
}