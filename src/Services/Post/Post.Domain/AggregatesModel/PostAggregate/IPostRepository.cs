using Post.Domain.Dtos.Post;

namespace Post.Domain.AggregatesModel.PostAggregate;

public interface IPostRepository
{
    Task<Post> Add(CreateOrUpdatePostDto post);
    Task Update(CreateOrUpdatePostDto postDto, string postId);
    Task<Post> FindByIdAsync(string id);
    Task<Post> FindBySlugAsync(string slug);

    Task Remove(Post post);
}
