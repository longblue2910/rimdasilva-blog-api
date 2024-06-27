using MediatR;
using Post.Domain.AggregatesModel.UserAggregate;

namespace Post.Api.Applications.Commands.User;

public class CreateUserCommand : IRequest<bool>
{
    public string Username { get; set; }
    public string FullName { get; set; }
    public string Password { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public int UserType { get; set; }
}
