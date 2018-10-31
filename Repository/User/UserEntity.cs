using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using Repository.Core;

namespace Repository.User
{
    [BsonIgnoreExtraElements]
    public class UserEntity : BaseEntity
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public List<string> Roles { get; set; }
    }
}