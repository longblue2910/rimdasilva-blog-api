using Post.Api.Apis;
using Post.Api.Models;

namespace Post.Api.Applications.Queries.Post;

public interface IPostQueries
{
    Task<Domain.AggregatesModel.PostAggregate.Post> FindByIdAsync(string id);
    Task<Domain.AggregatesModel.PostAggregate.Post> FindBySlugAsync(string slug, bool? userWatch);
    Task<PaginatedItems<PostDto>> GetPostsAsync(PaginationRequest paginationRequest, SearchPostRequest request);

    Task<HomePostDto> GetHomePostAsync();
}
