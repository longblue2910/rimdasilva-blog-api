using Contracts.Common.Interfaces;
using Post.Domain.AggregatesModel.UserAggregate;

namespace Post.Infrastructure.Repositories;

public class UserRepository(IRepositoryBaseAsync<User, PostDbContext> repositoryBaseAsync) : IUserRepository
{
    private readonly IRepositoryBaseAsync<User, PostDbContext> _repositoryBaseAsync = repositoryBaseAsync;

    public User Add(User user)
    {
        throw new NotImplementedException();
    }

    public Task<User> FindByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public User Update(User user)
    {
        throw new NotImplementedException();
    }
}
