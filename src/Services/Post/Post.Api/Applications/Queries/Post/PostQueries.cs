using MongoDB.Driver;
using Post.Api.Apis;
using Post.Api.Models;
using Post.Domain.AggregatesModel.PostAggregate;

namespace Post.Api.Applications.Queries.Post;

public class PostQueries : IPostQueries
{
    private readonly IPostRepository _repository;

    public PostQueries(IPostRepository repository)
    {
        _repository = repository;
    }

    public async Task<Domain.AggregatesModel.PostAggregate.Post> FindByIdAsync(string id)
    {
        return await _repository.FindByIdAsync(id);
    }

    public async Task<HomePostDto> GetHomePostAsync()
    {
        HomePostDto response = new();

        // Lấy bài viết mới nhất với thể loại "News"
        var filterNews = Builders<Domain.AggregatesModel.PostAggregate.Post>.Filter.ElemMatch(p => p.Categories, c => c.Title == "News");
        var latestNews = await _repository.GetPostsAsync(filterNews, 0, 1); // Lấy 1 bài viết

        response.LatestNews = latestNews.FirstOrDefault() != null ? MapToPostDto(latestNews.FirstOrDefault()) : null;

        // Lấy 5 bài viết mới nhất
        var latestBlogFilter = Builders<Domain.AggregatesModel.PostAggregate.Post>.Filter.Empty;
        var latestBlogs = await _repository.GetPostsAsync(latestBlogFilter, 0, 5);

        response.LatestBlog = latestBlogs.Select(MapToPostDto).ToList();

        // Lấy 5 bài viết đọc nhiều nhất
        var mostReadFilter = Builders<Domain.AggregatesModel.PostAggregate.Post>.Filter.Empty;
        var mostReadPosts = await _repository.GetPostsAsync(mostReadFilter, 0, 5);

        response.MostRead = mostReadPosts.OrderByDescending(p => p.CountWatch).Select(MapToPostDto).ToList();

        return response;
    }

    // Phương thức để chuyển đổi từ Post thành PostDto
    private PostDto MapToPostDto(Domain.AggregatesModel.PostAggregate.Post post)
    {
        return new PostDto
        {
            Id = post.Id.ToString(),
            Title = post.Title,
            DescriptionShort = post.DescriptionShort,
            ImageUrl = post.ImageUrl,
            Slug = post.Slug,
            CreatedDate = post.CreatedDate,
            Categories = post.Categories?.Select(c => new CategoryDto
            {
                Id = c.Id,
                Title = c.Title
            }).ToList() ?? new List<CategoryDto>()
        };
    }

    public async Task<Domain.AggregatesModel.PostAggregate.Post> FindBySlugAsync(string slug, bool? userWatch)
    {
        var post = await _repository.FindBySlugAsync(slug);

        if (userWatch == true)
        {
            await _repository.IncrementViewCountAsync(post.Id); // Increment view count
        }

        return post;
    }

    public async Task<PaginatedItems<PostDto>> GetPostsAsync(PaginationRequest paginationRequest, SearchPostRequest request)
    {
        var pageSize = paginationRequest.PageSize;
        var pageIndex = paginationRequest.PageIndex;
        var offSet = (pageIndex - 1) * pageSize;

        // Tạo filter tìm kiếm bài viết
        var filter = Builders<Domain.AggregatesModel.PostAggregate.Post>.Filter.Empty;

        // Lọc theo CategoryIds nếu có
        if (request.CategoryIds != null && request.CategoryIds.Any())
        {
            filter = filter & Builders<Domain.AggregatesModel.PostAggregate.Post>
                .Filter.ElemMatch(post => post.Categories, category => request.CategoryIds.Contains(category.Id));
        }

        // Lọc theo Title nếu có
        if (!string.IsNullOrEmpty(request.Title))
        {
            filter = filter & Builders<Domain.AggregatesModel.PostAggregate.Post>.Filter.Regex(post => post.Title, new MongoDB.Bson.BsonRegularExpression(request.Title, "i"));
        }

        // Lọc theo Slug nếu có
        if (!string.IsNullOrEmpty(request.Slug))
        {
            filter = filter & Builders<Domain.AggregatesModel.PostAggregate.Post>.Filter.Eq(post => post.Slug, request.Slug);
        }

        // Lấy tổng số bài viết thỏa mãn điều kiện
        var totalItems = await _repository.GetTotalItemsAsync(filter);

        // Lấy các bài viết theo phân trang
        var posts = await _repository.GetPostsAsync(filter, offSet, pageSize);

        // Chuyển đổi Post sang PostDto
        var postDtos = posts.Select(post => new PostDto
        {
            Id = post.Id.ToString(),
            Title = post.Title,
            DescriptionShort = post.DescriptionShort,
            ImageUrl = post.ImageUrl,
            Slug = post.Slug,
            CreatedDate = post.CreatedDate,
            Categories = post.Categories?.Select(c => new CategoryDto
            {
                Id = c.Id,
                Title = c.Title
            }).ToList() ?? new List<CategoryDto>()
        }).ToList();

        return new PaginatedItems<PostDto>(pageIndex, pageSize, totalItems, postDtos);
    }
}
