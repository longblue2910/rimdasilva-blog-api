using MongoDB.Driver;
using Post.Domain.AggregatesModel.CategoryAggregate;
using Post.Domain.AggregatesModel.PostAggregate;

namespace Post.Infrastructure.Repositories;

public class PostRepository(MongoDbContext context) : IPostRepository
{
    private readonly IMongoCollection<Post.Domain.AggregatesModel.PostAggregate.Post> _posts = context.Posts;
    private readonly IMongoCollection<Category> _categories = context.Categories;

    public async Task<Domain.AggregatesModel.PostAggregate.Post> Add(Post.Domain.AggregatesModel.PostAggregate.Post post)
    {
        // Kiểm tra nếu title đã tồn tại
        var existingPost = await _posts.Find(x => x.Title == post.Title).FirstOrDefaultAsync();
        if (existingPost != null) throw new InvalidOperationException($"A post with the title '{post.Title}' already exists.");

        await _posts.InsertOneAsync(post);
        return post;
    }

    public async Task AddCategorieByPost(Domain.AggregatesModel.PostAggregate.Post postEntity, List<string> ids)
    {
        var listCate = await _categories.Find(x => ids.Contains(x.Id)).ToListAsync();

        foreach (var item in listCate)
        {
            // Check if category already exists in post
            var isExist = postEntity.Categories.Any(x => x.Id == item.Id);
            if (!isExist) postEntity.Categories.Add(item);
        }

        // Update post with new categories
        await _posts.ReplaceOneAsync(x => x.Id == postEntity.Id, postEntity);
    }

    public async Task<Post.Domain.AggregatesModel.PostAggregate.Post> FindByIdAsync(string id)
    {
        return await _posts.Find(x => x.Id.ToString() == id).FirstOrDefaultAsync();
    }

    public async Task<Post.Domain.AggregatesModel.PostAggregate.Post> FindBySlugAsync(string slug)
    {
        return await _posts.Find(x => x.Slug == slug).FirstOrDefaultAsync();
    }

    public async Task Remove(Post.Domain.AggregatesModel.PostAggregate.Post post)
    {
        await _posts.DeleteOneAsync(x => x.Id == post.Id);
    }

    public async Task<Post.Domain.AggregatesModel.PostAggregate.Post> Update(Post.Domain.AggregatesModel.PostAggregate.Post postEntity)
    {
        await _posts.ReplaceOneAsync(x => x.Id == postEntity.Id, postEntity);
        return postEntity;
    }

    // Phương thức lấy bài viết với phân trang và tìm kiếm
    public async Task<long> GetTotalItemsAsync(FilterDefinition<Post.Domain.AggregatesModel.PostAggregate.Post> filter)
    {
        return await _posts.CountDocumentsAsync(filter);
    }

    public async Task<List<Post.Domain.AggregatesModel.PostAggregate.Post>> GetPostsAsync(FilterDefinition<Post.Domain.AggregatesModel.PostAggregate.Post> filter, int skip, int limit)
    {
        return await _posts.Find(filter)
                             .Skip(skip)
                             .Limit(limit)
                             .ToListAsync();
    }
}
