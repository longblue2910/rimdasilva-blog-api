namespace Post.Domain.AggregatesModel.CommentAggregate;

public interface ICommentRepository
{
    Task<Comment> Add(Comment comment);
    Task Update(Comment comment);
    Task Remove(Comment comment);
    Task<IEnumerable<Comment>> FindByIdAsync(Guid postId);
}
