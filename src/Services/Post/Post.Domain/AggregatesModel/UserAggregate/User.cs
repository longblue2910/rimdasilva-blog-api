namespace Post.Domain.AggregatesModel.UserAggregate;

public class User : EntityAuditBase<Guid>
{
    public string Username { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Password { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string Avatar { get; set; }
    public UserType UserType { get; set; }
}
