namespace Post.Domain.AggregatesModel.PostAggregate;

public interface IPostRepository
{
    Task<Post> Add(Post post);
    Task Update(Post post);
    Task<Post> FindByIdAsync(string id);
    Task<Post> FindBySlugAsync(string slug);

    Task Remove(Post post);
}
