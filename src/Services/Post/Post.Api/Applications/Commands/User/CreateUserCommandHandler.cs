using AutoMapper;
using MediatR;
using Post.Domain.AggregatesModel.UserAggregate;
using Post.Domain.Exceptions;

namespace Post.Api.Applications.Commands.User;

public class CreateUserCommandHandler(IUserRepository repository, IMapper mapper) : IRequestHandler<CreateUserCommand, bool>
{
    private readonly IUserRepository _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<bool> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        // Kiểm tra xem người dùng đã tồn tại hay chưa
        if (!(await _repository.IsUserValidForCreation(request.Username))) 
            throw new PostDomainException("Người dùng đã tồn tại.");

        var user = _mapper.Map<Domain.AggregatesModel.UserAggregate.User>(request);
        await _repository.Add(user);

        return true;
    }
}
