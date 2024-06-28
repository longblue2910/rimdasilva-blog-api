using Contracts.Common.Interfaces;
using Post.Domain.AggregatesModel.CategoryAggregate;
using Post.Domain.AggregatesModel.PostAggregate;

namespace Post.Infrastructure.Repositories;

public class PostRepository(IRepositoryBaseAsync<Domain.AggregatesModel.PostAggregate.Post, PostDbContext> repositoryBase,
    IRepositoryBaseAsync<Category, PostDbContext> categoryRepo, IUnitOfWork<PostDbContext> unitOfWork, PostDbContext context) : IPostRepository
{
    private readonly IRepositoryBaseAsync<Domain.AggregatesModel.PostAggregate.Post, PostDbContext> _repositoryBase = repositoryBase;
    private readonly IRepositoryBaseAsync<Category, PostDbContext> _categoryRepo = categoryRepo;
    private readonly IUnitOfWork<PostDbContext> _unitOfWork = unitOfWork;
    private readonly PostDbContext _context = context;

    public async Task<Domain.AggregatesModel.PostAggregate.Post> Add(Domain.AggregatesModel.PostAggregate.Post post)
    {     
        await _repositoryBase.CreateAsync(post);
        return post;
    }

    public async Task AddCategorieByPost(Domain.AggregatesModel.PostAggregate.Post postEntity, List<Guid> Ids)
    {
        await _context.Entry(postEntity)
                .Collection(i => i.Categories).LoadAsync();

        var listCate = _categoryRepo.FindByCondition(x => Ids.Contains(x.Id)).ToList();

        foreach (var item in listCate)
        {
            //Check exist
            var isExist = postEntity.Categories.Any(x => x.Id == item.Id);
            if (!isExist)
            {
                postEntity.Categories.Add(item);
            }
        }

        await _unitOfWork.CommitAsync();
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

    public async Task<Domain.AggregatesModel.PostAggregate.Post> Update(Domain.AggregatesModel.PostAggregate.Post postEntity)
    {     
        await _repositoryBase.UpdateAsync(postEntity, x => x.Id == postEntity.Id);
        return postEntity;
    }
}
