using MongoDB.Driver;
namespace Post.Domain.AggregatesModel.PostAggregate;

public interface IPostRepository
{
    Task<Post> Add(Post post);
    Task<Post> Update(Post postDto);
    Task<Post> FindByIdAsync(string id);
    Task<Post> FindBySlugAsync(string slug);
    Task Remove(Post post);
    Task AddCategorieByPost(Post post, List<string> Ids);

    // Lấy tổng số bài viết thỏa mãn filter
    Task<long> GetTotalItemsAsync(FilterDefinition<Post> filter);

    // Lấy các bài viết theo phân trang và filter
    Task<List<Post>> GetPostsAsync(FilterDefinition<Post> filter, int skip, int limit);
}
