using System.ComponentModel.DataAnnotations.Schema;

namespace Post.Domain.AggregatesModel.CourseAggregate;

public class Section : EntityAuditBase<Guid>
{
    public string SectionName { get; set; }

    public Guid CourseId { get; set; }

    [ForeignKey("CourseId")]
    public virtual Course Course { get; set; }

    public virtual ICollection<Lesson> Lessons { get; set; }
}
