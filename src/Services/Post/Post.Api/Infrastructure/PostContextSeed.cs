using EFCore.BulkExtensions;
using Post.Domain.AggregatesModel.CategoryAggregate;
using Post.Domain.AggregatesModel.UserAggregate;
using Post.Infrastructure;

namespace Post.Api.Infrastructure;

public class PostContextSeed : IDbSeeder<PostDbContext>
{
    public async Task SeedAsync(PostDbContext context)
    {
        
        if (!context.Categories.Any())
        {
            context.Categories.AddRange(GetCategories());
        }

        await context.SaveChangesAsync();
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