using Microsoft.AspNetCore.Identity;

namespace Post.Domain.AggregatesModel.UserAggregate;

public class ApplicationUser : IdentityUser
{
    public string FullName { get; set; }
    public UserType UserType { get; set; }
}
