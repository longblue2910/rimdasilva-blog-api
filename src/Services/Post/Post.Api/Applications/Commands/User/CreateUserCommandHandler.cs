using MediatR;

namespace Post.Api.Applications.Commands.User;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, bool>
{
    public CreateUserCommandHandler()
    {
        
    }
    public Task<bool> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
