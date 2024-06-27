using Post.Domain.AggregatesModel.UserAggregate;
using System.ComponentModel.DataAnnotations.Schema;

namespace Post.Domain.AggregatesModel.CommentAggregate;

public class Comment : EntityAuditBase<Guid>
{
    public string Content { get; set; }
    public string UserId { get; set; }
    public Guid PostId { get; set; }
    [ForeignKey("PostId")]
    public virtual PostAggregate.Post Post { get; set; }

}
