using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Repository.Core
{
    public class BaseEntity
    {
        [BsonId]
        public ObjectId Id { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime Created { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime? Updated { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime? Deleted { get; set; }
    }
}