namespace Post.Domain.AggregatesModel.UserAggregate;

public interface IUserRepository
{
    Task<User> Add(User user);
    Task Update(User user);
    Task<User> FindByIdAsync(string id);
    Task<User> FindByUsernameAsync(string username);

    /// <summary>
    /// Kiểm tra xem người dùng đã tồn tại hay chưa
    /// </summary>
    /// <param name="userName"></param>
    /// <returns></returns>
    Task<bool> IsUserValidForCreation(string userName);
}
