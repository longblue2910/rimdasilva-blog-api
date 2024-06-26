using Contracts.Common.Interfaces;
using Contracts.Interfaces;
using Post.Domain.AggregatesModel.UserAggregate;

namespace Post.Infrastructure.Repositories;

public class UserRepository(IRepositoryBaseAsync<User, PostDbContext> repositoryBaseAsync,
    IUnitOfWork<PostDbContext> unitOfWork, 
    IPasswordHasher passwordHasher) : IUserRepository
{
    private readonly IRepositoryBaseAsync<User, PostDbContext> _repositoryBaseAsync = repositoryBaseAsync;
    private readonly IUnitOfWork<PostDbContext> _unitOfWork = unitOfWork;
    private readonly IPasswordHasher _passwordHasher = passwordHasher;

    public async Task<User> Add(User user)
    {
        //Hash password PBKDF2
        user.Password = _passwordHasher.HashPassword(user.Password);
        await  _repositoryBaseAsync.CreateAsync(user);

        return user;
    }

    public async Task<User> FindByIdAsync(Guid id)
    {
        return await _repositoryBaseAsync.FindOneAsync(x => x.Id == id);
    }

    public async Task<bool> IsUserValidForCreation(string userName)
    {
        return await _repositoryBaseAsync.FindOneAsync(x => x.Username == userName) == null;
    }

    public async Task Update(User user)
    {
        await _repositoryBaseAsync.UpdateAsync(user, user.Id);
    }
}
