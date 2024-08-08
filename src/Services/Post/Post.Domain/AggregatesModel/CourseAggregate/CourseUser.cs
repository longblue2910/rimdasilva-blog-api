namespace Post.Domain.AggregatesModel.CourseAggregate;

public class CourseStudent : EntityAuditBase<Guid>
{
    public Guid CourseId { get; set; }
    public Guid StudentId { get; set; }

}
