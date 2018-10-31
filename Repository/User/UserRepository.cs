using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Core.User;
using MongoDB.Driver;
using Repository.Core;

namespace Repository.User
{
    public class UserRepository : IUserRepository
    {
        private readonly IDatabaseContext _context;
        private readonly IMapper _mapper;
        
        public UserRepository(IDatabaseContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

       public async Task<IEnumerable<UserModel>> FindAsync()
        {
            var entities = await _context.Users
                                         .Find(u => !u.Roles.Contains("admin") &&
                                                     u.Deleted == null)
                                         .ToListAsync();

            var users = _mapper.Map<List<UserModel>>(entities);
            return users;
        }

        public async Task<UserModel> FindByIdAsync(string id)
        {
            var entity = await _context.Users
                                       .Find(u => u.Id == id.ToObjectId())
                                       .FirstOrDefaultAsync();

            if (entity == null)
                return null;

            var user = _mapper.Map<UserModel>(entity);

            return user;
        }

        public async Task<UserModel> FindByEmailAsync(string email)
        {
            var entity = await _context.Users
                                       .Find(u => u.Email.ToUpperInvariant() == email.ToUpperInvariant())
                                       .FirstOrDefaultAsync();

            if (entity == null)
                return null;

            var user = _mapper.Map<UserModel>(entity);

            return user;
        }

        public async Task<UserModel> InsertAsync(UserModel user)
        {
            var entity = _mapper.Map<UserEntity>(user);

            entity.Roles = new List<string>
            {
                "user"
            };
            entity.Created = DateTime.UtcNow;

            await _context.Users.InsertOneAsync(entity);

            return await FindByEmailAsync(user.Email);
        }

        public async Task<bool> UpdateAsync(UserModel user)
        {
            var filter = Builders<UserEntity>.Filter.Eq(s => s.Id, user.Id.ToObjectId());
            var update = Builders<UserEntity>.Update
                                             .Set(s => s.Password, user.Password)
                                             .Set(s => s.Name, user.Name)
                                             .Set(s => s.Updated, DateTime.UtcNow);
            
            var result = await _context.Users.UpdateOneAsync(filter, update);

            return result.IsModifiedCountAvailable && result.ModifiedCount > 0;
        }

        public async Task<bool> DeleteAsync(string userId)
        {
            var filter = Builders<UserEntity>.Filter.Eq(s => s.Id, userId.ToObjectId());
            var update = Builders<UserEntity>.Update
                                             .Set(s => s.Deleted, DateTime.UtcNow);

            var result = await _context.Users.UpdateOneAsync(filter, update);
            
            return result.IsModifiedCountAvailable && result.ModifiedCount > 0;
        }
    }
}