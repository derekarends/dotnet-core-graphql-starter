using MongoDB.Driver;
using Repository.User;

namespace Repository.Core
{
    public interface IDatabaseContext
    {
        IMongoCollection<UserEntity> Users { get; }
    }
}