using Contracts.Common.Interfaces;
using Post.Domain.AggregatesModel.CategoryAggregate;
using Post.Domain.AggregatesModel.PostAggregate;
using Post.Domain.Dtos.Post;

namespace Post.Infrastructure.Repositories;

public class PostRepository(IRepositoryBaseAsync<Domain.AggregatesModel.PostAggregate.Post, PostDbContext> repositoryBase,
    IRepositoryBaseAsync<Category, PostDbContext> categoryRepo, IUnitOfWork<PostDbContext> unitOfWork) : IPostRepository
{
    private readonly IRepositoryBaseAsync<Domain.AggregatesModel.PostAggregate.Post, PostDbContext> _repositoryBase = repositoryBase;
    private readonly IRepositoryBaseAsync<Category, PostDbContext> _categoryRepo = categoryRepo;
    private readonly IUnitOfWork<PostDbContext> _unitOfWork = unitOfWork;

    public async Task<Domain.AggregatesModel.PostAggregate.Post> Add(CreateOrUpdatePostDto postDto)
    {
        var id = Guid.NewGuid();

        var postEntity = new Domain.AggregatesModel.PostAggregate.Post
        {
            Id = id,
            Slug = postDto.Slug,
            Title = postDto.Title,
            ImageUrl = postDto.ImageUrl,
            Description = postDto.Description,
        };

        _repositoryBase.Create(postEntity);
        await _unitOfWork.CommitAsync();

        var listCate = _categoryRepo.FindByCondition(x => postDto.CategoryIds.Contains(x.Id)).ToList();
        postEntity.Categories = listCate;

        await _unitOfWork.CommitAsync();
        return postEntity;
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

    public async Task Update(CreateOrUpdatePostDto postDto, string postId)
    {
        var postEntity = new Domain.AggregatesModel.PostAggregate.Post
        {
            Slug = postDto.Slug,

        };

        await _repositoryBase.UpdateAsync(postEntity, postId.ToString());

    }
}
