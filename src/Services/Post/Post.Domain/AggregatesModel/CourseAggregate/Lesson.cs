using MongoDB.Bson.Serialization.Attributes;
using Post.Domain.SeedWork;

namespace Post.Domain.AggregatesModel.CourseAggregate;

public class Lesson : MongoEntity
{
    public Lesson()
    {
        // Đặt CreatedDate
        CreatedDate = DateTime.UtcNow;
    }

    [BsonElement("lessonName")]
    public string LessonName { get; set; }

    [BsonElement("lessonType")]

    public LessonType LessonType { get; set; }

    [BsonElement("description")]
    public string Description { get; set; }

    [BsonElement("videoUrl")]
    public string VideoUrl { get; set; }

    [BsonElement("orderIndex")]
    public int OrderIndex { get; set; }

    [BsonElement("sectionId")]
    public string SectionId { get; set; }

    //[BsonElement("questions")]

    //public virtual List<Question> Questions { get; set; } = [];
}


public enum LessonType
{
    /// <summary>
    /// Lý thuyết
    /// </summary>
    Theory = 1,
    /// <summary>
    /// Bài tập
    /// </summary>
    Practice = 2,
    /// <summary>
    /// Trắc nghiệm
    /// </summary>
    Quiz = 3,
}