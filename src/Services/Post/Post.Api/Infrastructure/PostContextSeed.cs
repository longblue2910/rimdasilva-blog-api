using MongoDB.Driver;
using Post.Domain.AggregatesModel.CategoryAggregate;
using Post.Infrastructure;

namespace Post.Api.Infrastructure;

public class PostContextSeed(MongoDbContext context)
{
    private readonly IMongoCollection<Category> _categoryCollection = context.Categories;

    public async Task SeedAsync()
    {
        // Kiểm tra xem collection Category đã có dữ liệu chưa
        var existingCategories = await _categoryCollection.Find(FilterDefinition<Category>.Empty).ToListAsync();

        if (existingCategories.Count == 0)
        {
            // Nếu chưa có, thêm các category vào collection
            await _categoryCollection.InsertManyAsync(GetCategories());
        }
    }

    private static IEnumerable<Category> GetCategories()
    {
        return
        [
            new Category
            {
                Title = "C#",
                Slug = "csharp",
                ImgUrl = "csharp.png",
                OrderIndex = 1,
                TagName = "#CSharp"
            },
            new Category
            {
                Title = ".Net Core",
                Slug = "netcore",
                ImgUrl = "netcore.png",
                OrderIndex = 2,
                TagName = "#NetCore"
            },
            new Category
            {
                Title = "SQL",
                Slug = "sql",
                ImgUrl = "sql.png",
                OrderIndex = 3,
                TagName = "#SQL"
            },
            new Category
            {
                Title = "DevOps",
                Slug = "devops",
                ImgUrl = "devops.png",
                OrderIndex = 4,
                TagName = "#DevOps"
            },
            new Category
            {
                Title = "Design",
                Slug = "design",
                ImgUrl = "design.png",
                OrderIndex = 5,
                TagName = "#Design"
            },
            new Category
            {
                Title = "Khác",
                Slug = "other",
                ImgUrl = "other.png",
                OrderIndex = 6,
                TagName = "#Khác"
            }
        ];
    }
}
