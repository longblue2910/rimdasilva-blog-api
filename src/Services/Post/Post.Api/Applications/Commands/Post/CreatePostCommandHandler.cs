using AutoMapper;
using MediatR;
using Post.Domain.AggregatesModel.PostAggregate;

namespace Post.Api.Applications.Commands.Post;

public class CreatePostCommandHandler(IPostRepository repository, IMapper mapper) : IRequestHandler<CreatePostCommand, bool>
{
    private readonly IPostRepository _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<bool> Handle(CreatePostCommand request, CancellationToken cancellationToken)
    {
        var postEntity = _mapper.Map<Domain.AggregatesModel.PostAggregate.Post>(request);
        await _repository.Add(postEntity);

        return true;
    }
}
