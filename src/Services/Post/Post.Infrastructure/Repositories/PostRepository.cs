using Contracts.Common.Interfaces;
using Post.Domain.AggregatesModel.PostAggregate;

namespace Post.Infrastructure.Repositories;

public class PostRepository(IRepositoryBaseAsync<Domain.AggregatesModel.PostAggregate.Post, PostDbContext> repositoryBase) : IPostRepository
{
    private readonly IRepositoryBaseAsync<Domain.AggregatesModel.PostAggregate.Post, PostDbContext> _repositoryBase = repositoryBase;

    public async Task<Domain.AggregatesModel.PostAggregate.Post> Add(Domain.AggregatesModel.PostAggregate.Post post)
    {
        await _repositoryBase.CreateAsync(post);
        return post;
    }

    public async Task<Domain.AggregatesModel.PostAggregate.Post> FindByIdAsync(string id)
    {
        return await _repositoryBase.FindOneAsync(x => x.Id.ToString() == id);
    }

    public async Task<Domain.AggregatesModel.PostAggregate.Post> FindBySlugAsync(string slug)
    {
        return await _repositoryBase.FindOneAsync(x => x.Slug == slug);
    }

    public async Task Remove(Domain.AggregatesModel.PostAggregate.Post post)
    {
        await _repositoryBase.DeleteAsync(post);
    }

    public async Task Update(Domain.AggregatesModel.PostAggregate.Post post)
    {
        await _repositoryBase.UpdateAsync(post, post.Id.ToString());

    }
}
