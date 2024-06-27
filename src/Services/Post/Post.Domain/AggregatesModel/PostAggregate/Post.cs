using Post.Domain.AggregatesModel.CategoryAggregate;
using Post.Domain.AggregatesModel.CommentAggregate;

namespace Post.Domain.AggregatesModel.PostAggregate;

public class Post : EntityAuditBase<Guid>
{
    public string Title { get; set; }
    public string Description { get; set; }
    public string ImageUrl { get; set; }
    public string Slug { get; set; }
    public virtual ICollection<Category> Categories { get; set; }
    public virtual ICollection<Comment> Comments { get; set; }

}
