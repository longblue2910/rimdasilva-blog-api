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
                Title = "News",
                Slug = "news",
                OrderIndex = 1,
            },
            new Category
            {
                Title = "Series",
                Slug = "series",
                OrderIndex = 2,
            },
            new Category
            {
                Title = ".Net Core",
                Slug = "netcore",
                OrderIndex = 3,
            },
            new Category
            {
                Title = "SQL",
                Slug = "sql",
                OrderIndex = 4,
            },
            new Category
            {
                Title = "SQL",
                Slug = "sql",
                OrderIndex = 5,
            },
            new Category
            {
                Title = "DevOps",
                Slug = "devops",
                OrderIndex = 6,
            },
            new Category
            {
                Title = "Other",
                Slug = "other",
                ImgUrl = "other.png",
                OrderIndex = 7,
                TagName = "#Khác"
            }
        ];
    }
}
