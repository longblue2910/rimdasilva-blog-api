using MongoDB.Driver;
using Post.Domain.AggregatesModel.CategoryAggregate;

namespace Post.Infrastructure.Repositories
{
    public class CategoryRepository(MongoDbContext context) : ICategoryRepository
    {
        private readonly IMongoCollection<Category> _categories = context.Categories;

        // Thêm một category vào MongoDB
        public async Task<Category> Add(Category category)
        {
            await _categories.InsertOneAsync(category);
            return category;
        }

        // Lấy danh sách categories với giới hạn size
        public async Task<List<Category>> GetCategories(int size)
        {
            return await _categories.Find(FilterDefinition<Category>.Empty)
                .SortBy(x => x.OrderIndex)
                .Limit(size)
                .ToListAsync();
        }

        // Cập nhật category
        public async Task Update(Category category)
        {
            await _categories.ReplaceOneAsync(x => x.Id == category.Id, category);
        }
    }
}
