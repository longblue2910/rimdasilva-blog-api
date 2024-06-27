using MediatR;

namespace Post.Api.Applications.Commands.Post;

public class CreatePostCommand : IRequest<bool>
{
    public string Title { get; set; }
    public string Description { get; set; }
    public IFormFile ImageFile { get; set; }
    public string Slug { get; set; }
}
