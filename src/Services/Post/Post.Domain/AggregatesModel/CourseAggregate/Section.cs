using MongoDB.Bson.Serialization.Attributes;
using Post.Domain.SeedWork;

namespace Post.Domain.AggregatesModel.CourseAggregate;

public class Section : MongoEntity
{
    public Section()
    {
        // Đặt CreatedDate
        CreatedDate = DateTime.UtcNow;
    }

    [BsonElement("sectionName")]
    public string SectionName { get; set; }

    [BsonElement("courseId")]
    public string CourseId { get; set; }

    [BsonElement("lessons")]
    public virtual List<Lesson> Lessons { get; set; } = [];
}
