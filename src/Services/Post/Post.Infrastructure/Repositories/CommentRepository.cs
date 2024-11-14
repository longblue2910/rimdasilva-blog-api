using MongoDB.Driver;
using Post.Domain.AggregatesModel.CommentAggregate;

namespace Post.Infrastructure.Repositories;

public class CommentRepository(MongoDbContext context) : ICommentRepository
{
    private readonly IMongoCollection<Comment> _comments = context.Comments;

    public async Task<Comment> Add(Comment comment)
    {
        await _comments.InsertOneAsync(comment);
        return comment;
    }

    public async Task<IEnumerable<Comment>> FindByIdAsync(string postId)
    {
        return await _comments.Find(x => x.PostId == postId).ToListAsync();
    }

    public async Task Remove(Comment comment)
    {
        await _comments.DeleteOneAsync(x => x.Id == comment.Id);
    }

    public async Task Update(Comment comment)
    {
        await _comments.ReplaceOneAsync(x => x.Id == comment.Id, comment);
    }
}
