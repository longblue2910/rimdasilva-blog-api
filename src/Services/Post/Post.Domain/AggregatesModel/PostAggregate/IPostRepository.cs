namespace Post.Domain.AggregatesModel.PostAggregate;

public interface IPostRepository
{
    Task<Post> Add(Post post);
    Task<Post> Update(Post postDto);
    Task<Post> FindByIdAsync(string id);
    Task<Post> FindBySlugAsync(string slug);
    Task Remove(Post post);
    Task AddCategorieByPost(Post post, List<string> Ids);

}
