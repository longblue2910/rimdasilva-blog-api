namespace Post.Domain.AggregatesModel.CategoryAggregate;

public class Category : EntityAuditBase<Guid>
{
    public string Slug { get; set; }
    public string Title { get; set; }
    public string ImgUrl { get; set; }
    public int OrderIndex { get; set; }
    public string TagName { get; set; }
    public virtual ICollection<PostAggregate.Post> Posts { get; set; }

}
