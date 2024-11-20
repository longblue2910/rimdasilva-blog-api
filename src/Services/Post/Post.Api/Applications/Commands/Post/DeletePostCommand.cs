using Contracts.Exceptions;
using MediatR;
using Post.Domain.AggregatesModel.PostAggregate;

namespace Post.Api.Applications.Commands.Post;

public class DeletePostCommand : IRequest<string>
{    
    public string Id { get; set; }
}
public class DeletePostCommandHandler(IPostRepository repository) : IRequestHandler<DeletePostCommand, string>
{
    private readonly IPostRepository _repository = repository;

    public async Task<string> Handle(DeletePostCommand request, CancellationToken cancellationToken)
    {
        var post = await _repository.FindByIdAsync(request.Id) ?? throw new NotFoundException($"Post with id {request.Id} not found.");
        await _repository.Remove(post);

        return post.Id;
    }
}
