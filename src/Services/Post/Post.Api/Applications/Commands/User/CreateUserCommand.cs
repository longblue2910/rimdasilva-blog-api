using MediatR;

namespace Post.Api.Applications.Commands.User;

public class CreateUserCommand : IRequest<bool>
{
    public string Username { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Password { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
}
