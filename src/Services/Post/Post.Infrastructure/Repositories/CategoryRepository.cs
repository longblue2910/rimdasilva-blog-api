using Contracts.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using Post.Domain.AggregatesModel.CategoryAggregate;

namespace Post.Infrastructure.Repositories;

public class CategoryRepository(IRepositoryBaseAsync<Category, PostDbContext> repositoryBaseAsync,
    IUnitOfWork<PostDbContext> unitOfWork) : ICategoryRepository
{
    private readonly IRepositoryBaseAsync<Category, PostDbContext> _repositoryBaseAsync = repositoryBaseAsync;
    private readonly IUnitOfWork<PostDbContext> _unitOfWork = unitOfWork;

    public async Task<Category> Add(Category category)
    {
        await _repositoryBaseAsync.CreateAsync(category);
        return category;
    }

    public Task<List<Category>> GetCategories()
    {
        return _repositoryBaseAsync.FindByCondition(x => true).ToListAsync(cancellationToken: default);
    }

    public async Task Update(Category category)
    {
        await _repositoryBaseAsync.UpdateAsync(category, category.Id.ToString());
    }
}
