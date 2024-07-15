using MediatR;
using Microsoft.AspNetCore.SignalR;
using Post.Domain.AggregatesModel.CommentAggregate;
using Post.Domain.AggregatesModel.UserAggregate;

namespace Post.Api.Applications.Commands.Comment;

public class CommentCommandHandler(ICommentRepository repository, IUserRepository userRepository, IHubContext<NotificationHub> hubContext) : IRequestHandler<CommentCommand, bool>
{
    private readonly ICommentRepository _repository = repository;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IHubContext<NotificationHub> _hubContext = hubContext;

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

        // Gửi thông báo tới nhóm người dùng đã comment bài post này
        await _hubContext.Clients.Group(commentEntity.PostId.ToString())
            .SendAsync("ReceiveNotification", $"{user.FullName} đã bình luận: {commentEntity.Content}");

        return true;
    }
}
