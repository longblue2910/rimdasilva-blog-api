
using Post.Domain.AggregatesModel.PostAggregate;

namespace Post.Api.Applications.Queries.Post;

public class PostQueries(IPostRepository repository) : IPostQueries
{
    private readonly IPostRepository _repository = repository;

    public async Task<Domain.AggregatesModel.PostAggregate.Post> FindByIdAsync(string id)
    {
        return await _repository.FindByIdAsync(id);
    }

    public async Task<Domain.AggregatesModel.PostAggregate.Post> FindBySlugAsync(string slug)
    {
        return await _repository.FindBySlugAsync(slug);
    }
}
