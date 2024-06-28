using Post.Domain.AggregatesModel.CategoryAggregate;
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
                ImgUrl = "csharp.png",
                OrderIndex = 1,
            },
            new()
            {
                 Title = ".Net Core",
                 Slug = "netcore",
                 ImgUrl = "netcore.png",
                 OrderIndex= 2,
            },
            new()
            {
                Title = "SQL",
                Slug = "sql",
                ImgUrl = "sql.png"
            },
            new()
            {
                Title = "DevOps",
                Slug = "devops",
                ImgUrl = "devops.png"
            },
            new()
            {
                Title = "Design",
                Slug = "design",
                ImgUrl = "design.png"
            },
            new()
            {
                Title = "Khác",
                Slug = "other",
                ImgUrl = "other.png"
            },
        ];
    }

}