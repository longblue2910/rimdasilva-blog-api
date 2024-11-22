using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Post.Domain.SeedWork;
public abstract class MongoEntity
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    [BsonElement("_id")]
    public virtual string Id { get; set; }
    [BsonElement("createdDate")]
    public DateTime CreatedDate { get; set; } = DateTime.Now;
    [BsonElement("lastModifiedDate")]
    public DateTime? LastModifiedDate { get; set; } = DateTime.Now;
}
