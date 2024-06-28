using MediatR;

namespace Post.Api.Applications.Commands.Comment;

public class CommentCommand : IRequest<bool>
{
    public string Content { get; set; }
    public Guid PostId { get; set; }
    public string Username { get; set; }
}
