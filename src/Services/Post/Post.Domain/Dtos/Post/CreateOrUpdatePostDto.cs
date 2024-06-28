namespace Post.Domain.Dtos.Post;

public class CreateOrUpdatePostDto
{
    public string Title { get; set; }
    public string Description { get; set; }
    public string ImageUrl { get; set; }
    public string Slug { get; set; }
    public List<Guid> CategoryIds { get; set; } = [];
}
