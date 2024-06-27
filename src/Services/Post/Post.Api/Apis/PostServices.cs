using MediatR;
using Post.Api.Applications.Queries.Post;

namespace Post.Api.Apis;

public class PostServices(
    IMediator mediator,
    ILogger<PostServices> logger,
    IPostQueries postQueries)
{
    public IPostQueries PostQueries { get; set; } = postQueries;

    public IMediator Mediator { get; set; } = mediator;
    public ILogger<PostServices> Logger { get; } = logger;
}
