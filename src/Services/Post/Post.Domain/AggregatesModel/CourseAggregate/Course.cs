namespace Post.Domain.AggregatesModel.CourseAggregate;

public class Course : EntityAuditBase<Guid>
{
    public string CourseName { get; set; }

    /// <summary>
    /// Free | Premium
    /// </summary>
    public CourseType CourseType { get; set; }

    /// <summary>
    /// Phí
    /// </summary>
    public int? Tuition { get; set; }

    public string ImgUrl { get; set; }

    public virtual ICollection<Section> Sections { get; set; }

}

public enum CourseType
{
    Free = 1,
    Premium = 2
}