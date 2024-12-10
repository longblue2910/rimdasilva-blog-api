using MongoDB.Bson.Serialization.Attributes;
using Post.Domain.SeedWork;

namespace Post.Domain.AggregatesModel.CourseAggregate;

public class Course : MongoEntity
{
    public Course()
    {
        // Đặt CreatedDate
        CreatedDate = DateTime.UtcNow;
    }

    [BsonElement("courseName")]
    public string CourseName { get; set; }

    /// <summary>
    /// Free | Premium
    /// </summary> 
    [BsonElement("courseType")]
    public CourseType CourseType { get; set; }

    /// <summary>
    /// Phí
    /// </summary>
    [BsonElement("tuition")]
    public int? Tuition { get; set; }

    [BsonElement("imgUrl")]
    public string ImgUrl { get; set; }

    [BsonElement("sections")]
    public virtual List<Section> Sections { get; set; } = [];

}

public enum CourseType
{
    Free = 1,
    Premium = 2
}