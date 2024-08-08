namespace Post.Domain.AggregatesModel.CourseAggregate;

public class Lesson : EntityAuditBase<Guid>
{
    public string LessonName { get; set; }

    /// <summary>
    /// Loại bài học
    /// </summary>
    public LessonType LessonType { get; set; }

    public string Description { get; set; }
    public string VideoUrl { get; set; }
    public int OrderIndex { get; set; }
    public virtual ICollection<Question> Questions { get; set; } = [];
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