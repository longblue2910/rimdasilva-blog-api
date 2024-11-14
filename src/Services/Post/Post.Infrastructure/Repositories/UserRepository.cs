using MongoDB.Driver;
using Post.Domain.AggregatesModel.UserAggregate;

namespace Post.Infrastructure.Repositories;

public class UserRepository(MongoDbContext context) : IUserRepository
{
    private readonly IMongoCollection<User> _users = context.Users;

    public async Task<User> Add(User user)
    {
        await _users.InsertOneAsync(user);
        return user;
    }

    public async Task<User> FindByIdAsync(string id)
    {
        return await _users.Find(x => x.Id == id).FirstOrDefaultAsync();
    }

    public async Task<User> FindByUsernameAsync(string username)
    {
        return await _users.Find(x => x.Username == username).FirstOrDefaultAsync();
    }

    public async Task<bool> IsUserValidForCreation(string userName)
    {
        var existingUser = await _users.Find(x => x.Username == userName).FirstOrDefaultAsync();
        return existingUser == null;
    }

    public async Task Update(User user)
    {
        await _users.ReplaceOneAsync(x => x.Id == user.Id, user);
    }
}
