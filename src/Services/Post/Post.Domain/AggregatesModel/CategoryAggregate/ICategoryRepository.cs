namespace Post.Domain.AggregatesModel.CategoryAggregate;

public interface ICategoryRepository
{
    Task<Category> Add(Category category);
    Task Update(Category category);
    Task<List<Category>> GetCategories(int size);

}
