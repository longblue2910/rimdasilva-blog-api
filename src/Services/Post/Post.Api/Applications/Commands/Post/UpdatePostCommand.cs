namespace Post.Api.Applications.Commands.Post;

public class UpdatePostCommand : CreatePostCommand
{
    public Guid Id { get; set; }
}
