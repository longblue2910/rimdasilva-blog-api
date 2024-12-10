using MongoDB.Bson.Serialization.Attributes;
using Post.Domain.SeedWork;

namespace Post.Domain.AggregatesModel.CourseAggregate;

public class CourseStudent : MongoEntity
{
    [BsonElement("courseId")]
    public string CourseId { get; set; }

    [BsonElement("userId")]
    public string UserId { get; set; }

}
