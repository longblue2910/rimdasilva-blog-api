using System.ComponentModel.DataAnnotations.Schema;
namespace Post.Domain.AggregatesModel.CourseAggregate;

/// <summary>
/// Danh sách câu hỏi
/// </summary>
public class Question : EntityAuditBase<Guid>
{
    public string QuestionName { get; set; }
}

/// <summary>
/// Danh sách câu trả lời
/// </summary>
public class Solution : EntityAuditBase<Guid>
{
    public string SolutionName { get; set; }
    public Guid QuestionId { get; set; }

    [ForeignKey(nameof(QuestionId))]
    public virtual Question Question { get; set; }

    public bool? IsCorrect { get; set; }
}

/// <summary>
/// Danh sách câu trả lời
/// </summary>
public class SolutionByStudent : EntityAuditBase<Guid>
{
    public Guid QuestionId { get; set; }
    [ForeignKey(nameof(QuestionId))]
    public virtual Question Question { get; set; }
    public virtual ICollection<Solution> Solutions { get; set; }
    public Guid UserId { get; set; }
}

