using Contracts.Common.Interfaces;
using Microsoft.AspNetCore.Identity;
using Post.Domain.AggregatesModel.UserAggregate;

namespace Post.Infrastructure.Repositories;

public class UserRepository(IRepositoryBaseAsync<ApplicationUser, PostDbContext> repositoryBaseAsync,
    IUnitOfWork<PostDbContext> unitOfWork,
    UserManager<ApplicationUser> userManager) : IUserRepository
{
    private readonly IRepositoryBaseAsync<ApplicationUser, PostDbContext> _repositoryBaseAsync = repositoryBaseAsync;
    private readonly IUnitOfWork<PostDbContext> _unitOfWork = unitOfWork;
    private readonly UserManager<ApplicationUser> _userManager = userManager;

    public async Task<ApplicationUser> Add(ApplicationUser user)
    {
        await _userManager.CreateAsync(user);

        return user;
    }

    public async Task<ApplicationUser> FindByIdAsync(string id)
    {
        return await _repositoryBaseAsync.FindOneAsync(x => x.Id == id);
    }

    public async Task<ApplicationUser> FindByUsernameAsync(string username)
    {
        return await _repositoryBaseAsync.FindOneAsync(x => x.UserName == username);
    }

    public async Task<bool> IsUserValidForCreation(string userName)
    {
        return await _repositoryBaseAsync.FindOneAsync(x => x.UserName == userName) == null;
    }

    public async Task Update(ApplicationUser user)
    {
        await _repositoryBaseAsync.UpdateAsync(user, x => x.Id == user.Id);
    }
}
