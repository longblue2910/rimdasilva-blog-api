using EFCore.BulkExtensions;
using Post.Domain.AggregatesModel.CategoryAggregate;
using Post.Domain.AggregatesModel.UserAggregate;
using Post.Infrastructure;

namespace Post.Api.Infrastructure;

public class PostContextSeed : IDbSeeder<PostDbContext>
{
    public async Task SeedAsync(PostDbContext context)
    {
        if (!context.Users.Any())
        {
            context.Users.AddRange(GetUsers());
        }

        if (!context.Categories.Any())
        {
            context.Categories.AddRange(GetCategories());
        }

        await context.SaveChangesAsync();
    }
    private static IEnumerable<User> GetUsers()
    {
        return
        [
            new()
            {
                FullName = "Admin",
                Password = "admin",
                Username = "admin",
                Phone = "0983336103",
                Email = "tlong.admin@rimdasilva.com",
                UserType = UserType.Default,
                CreateBy = "admin"
            },
            new()
            {
                FullName = "Long Phan",
                Username = "longjr",
                Password = "longjr",
                Phone = "0983336104",
                Email = "long.jr@rimdasilva.com",
                UserType = UserType.Default,
                CreateBy = "admin"
            }
        ];
    }

    private static IEnumerable<Category> GetCategories()
    {
        return
        [
            new()
            {
                Title = "C#",
                Slug = "csharp",
                ImgUrl = "csharp.png"
            },
            new()
            {
                 Title = ".Net Core",
                 Slug = "netcore",
                 ImgUrl = "netcore.png"
            }
        ];
    }

}