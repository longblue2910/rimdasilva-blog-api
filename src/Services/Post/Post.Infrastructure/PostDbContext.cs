using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using Post.Domain.AggregatesModel.CategoryAggregate;
using Post.Domain.AggregatesModel.CommentAggregate;
using Post.Domain.AggregatesModel.UserAggregate;

namespace Post.Infrastructure;

public class MongoDbContext
{
    private readonly IMongoDatabase _database;

    public MongoDbContext(IConfiguration configuration)
    {
        var connectionString = configuration.GetValue<string>("MongoDbSettings:ConnectionString");
        var client = new MongoClient(connectionString);
        _database = client.GetDatabase("PostDb");
    }

    public IMongoCollection<Category> Categories => _database.GetCollection<Category>("Categories");
    public IMongoCollection<Comment> Comments => _database.GetCollection<Comment>("Comments");
    public IMongoCollection<Post.Domain.AggregatesModel.PostAggregate.Post> Posts => _database.GetCollection<Post.Domain.AggregatesModel.PostAggregate.Post>("Posts");
    public IMongoCollection<User> Users => _database.GetCollection<User>("Users");
}
