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
                TagName = "#CSharp"
            },
            new()
            {
                 Title = ".Net Core",
                 Slug = "netcore",
                 ImgUrl = "netcore.png",
                 OrderIndex = 2,
                 TagName = "#NetCore"

            },
            new()
            {
                Title = "SQL",
                Slug = "sql",
                ImgUrl = "sql.png",
                OrderIndex = 3,
                TagName = "#SQL"
            },
            new()
            {
                Title = "DevOps",
                Slug = "devops",
                ImgUrl = "devops.png",
                OrderIndex = 4,
                TagName = "#DevOps"
            },
            new()
            {
                Title = "Design",
                Slug = "design",
                ImgUrl = "design.png",
                OrderIndex = 5,
                TagName = "#Design"
            },
            new()
            {
                Title = "Khác",
                Slug = "other",
                ImgUrl = "other.png",
                OrderIndex = 6,
                TagName = "#Khác"
            },
        ];
    }

}