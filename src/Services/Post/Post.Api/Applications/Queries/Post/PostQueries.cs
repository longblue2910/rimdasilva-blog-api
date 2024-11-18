using MongoDB.Driver;
using Post.Api.Apis;
using Post.Api.Models;
using Post.Domain.AggregatesModel.PostAggregate;

namespace Post.Api.Applications.Queries.Post;

public class PostQueries(IPostRepository repository) : IPostQueries
{
    private readonly IPostRepository _repository = repository;

    public async Task<Domain.AggregatesModel.PostAggregate.Post> FindByIdAsync(string id)
    {
        return await _repository.FindByIdAsync(id);
    }

    public async Task<Domain.AggregatesModel.PostAggregate.Post> FindBySlugAsync(string slug)
    {
        return await _repository.FindBySlugAsync(slug);
    }

    public async Task<PaginatedItems<PostDto>> GetPostsAsync(PaginationRequest paginationRequest, SearchPostRequest request)
    {
        var pageSize = paginationRequest.PageSize;
        var pageIndex = paginationRequest.PageIndex;
        var offSet = pageIndex * pageSize - pageSize;

        // Tạo filter tìm kiếm bài viết
        var filter = Builders<Domain.AggregatesModel.PostAggregate.Post>.Filter.Empty;

        // Lọc theo CategoryId nếu có
        if (!string.IsNullOrEmpty(request.CategoryId))
        {
            filter = filter & Builders<Domain.AggregatesModel.PostAggregate.Post>.Filter.ElemMatch(post => post.Categories, category => category.Id == request.CategoryId);
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
            Id = post.Id,
            Title = post.Title,
            Description = post.Description,
            ImageUrl = post.ImageUrl,
            Slug = post.Slug,
            CreatedDate = post.CreatedDate,
            Categories = post.Categories?.Select(c => new CategoryDto
            {
                Id = c.Id,
                Title = c.Title
            }).ToList() ?? []
        }).ToList();

        return new PaginatedItems<PostDto>(pageIndex, pageSize, totalItems, postDtos);
    }
}
