using Contracts.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using Post.Domain.AggregatesModel.CommentAggregate;

namespace Post.Infrastructure.Repositories;

public class CommentRepository(IRepositoryBaseAsync<Comment, PostDbContext> repositoryBase) : ICommentRepository
{
    private readonly IRepositoryBaseAsync<Comment, PostDbContext> _repositoryBase = repositoryBase;

    public async Task<Comment> Add(Comment comment)
    {
        await _repositoryBase.CreateAsync(comment);
        return comment; 
    }

    public async Task<IEnumerable<Comment>> FindByIdAsync(Guid postId)
    {
        return await _repositoryBase.FindByCondition(x => x.PostId == postId).ToListAsync();
    }

    public async Task Remove(Comment comment)
    {
        await _repositoryBase.DeleteAsync(comment);
    }

    public async Task Update(Comment comment)
    {
        await _repositoryBase.UpdateAsync(comment, x => x.Id == comment.Id);
    }
}
