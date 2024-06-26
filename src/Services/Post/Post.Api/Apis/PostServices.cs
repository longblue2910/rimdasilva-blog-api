using MediatR;

namespace Post.Api.Apis;

public class PostServices(
    IMediator mediator,
    ILogger<PostServices> logger)
{
    public IMediator Mediator { get; set; } = mediator;
    public ILogger<PostServices> Logger { get; } = logger;
}
