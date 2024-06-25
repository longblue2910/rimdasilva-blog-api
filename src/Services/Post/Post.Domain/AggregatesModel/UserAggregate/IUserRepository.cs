namespace Post.Domain.AggregatesModel.UserAggregate;

public interface IUserRepository
{
    User Add(User user);
    User Update(User user);
    Task<User> FindByIdAsync(Guid id);
}
