using MediatR;
using Post.Domain.AggregatesModel.CommentAggregate;
using Post.Domain.AggregatesModel.UserAggregate;

namespace Post.Api.Applications.Commands.Comment;

public class CommentCommandHandler(ICommentRepository repository, IUserRepository userRepository) : IRequestHandler<CommentCommand, bool>
{
    private readonly ICommentRepository _repository = repository;
    private readonly IUserRepository _userRepository = userRepository;

    public async Task<bool> Handle(CommentCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.FindByUsernameAsync(request.Username);
        var commentEntity = new Domain.AggregatesModel.CommentAggregate.Comment
        {
            Content = request.Content,
            PostId  = request.PostId,
            UserId = user?.Id,
            CreatedDate = DateTime.Now,
        };

        await _repository.Add(commentEntity);
        return true;
    }
}
