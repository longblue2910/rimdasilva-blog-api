namespace Post.Domain.AggregatesModel.UserAggregate;

public interface IUserRepository
{
    Task<ApplicationUser> Add(ApplicationUser user);
    Task Update(ApplicationUser user);
    Task<ApplicationUser> FindByIdAsync(string id);
    Task<ApplicationUser> FindByUsernameAsync(string username);

    /// <summary>
    /// Kiểm tra xem người dùng đã tồn tại hay chưa
    /// </summary>
    /// <param name="userName"></param>
    /// <returns></returns>
    Task<bool> IsUserValidForCreation(string userName);
}
