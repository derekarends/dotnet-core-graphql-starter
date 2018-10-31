using Core.Settings;
using MongoDB.Driver;
using Repository.User;

namespace Repository.Core
{
    public class MongoContext : IDatabaseContext
    {
        private readonly IMongoDatabase _database;

        public MongoContext(SettingsModel settings)
        {
            var client = new MongoClient(settings.ConnectionStrings);
            _database = client.GetDatabase(settings.Database);
        }

        public IMongoCollection<UserEntity> Users => _database.GetCollection<UserEntity>("Users");
    }
}