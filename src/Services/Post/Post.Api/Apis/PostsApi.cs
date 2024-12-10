using Contracts.Common.Responses;
using Infrastructure.Extensions;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Post.Api.Applications.Commands.Categories;
using Post.Api.Applications.Commands.Comment;
using Post.Api.Applications.Commands.Post;
using Post.Api.Applications.Commands.User;
using Post.Api.Applications.Queries.Category;
using Post.Api.Models;
using Post.Domain.AggregatesModel.UserAggregate;
using static Contracts.Helper.DateTimeHelper;


namespace Post.Api.Apis;

public static class PostsApi
{
    public static RouteGroupBuilder MapUsersApiV1(this IEndpointRouteBuilder app)
    {
        var api = app.MapGroup("api/user").HasApiVersion(1.0);

        api.MapPost("/", CreateUserAsync);
        return api;
    }

    public static RouteGroupBuilder MapCategoriesApiV1(this IEndpointRouteBuilder app)
    {
        var api = app.MapGroup("api/category").HasApiVersion(1.0);

        api.MapGet("/", GetsCategoryAsync);
        api.MapPost("/", CreateCategoryAsync).DisableAntiforgery();

        return api;
    }

    public static RouteGroupBuilder MapPostsApiV1(this IEndpointRouteBuilder app)
    {
        var api = app.MapGroup("api/post").HasApiVersion(1.0);

        api.MapPost("/", CreatePostAsync).DisableAntiforgery();
        api.MapPut("/", UpdatePostAsync).DisableAntiforgery();
        api.MapDelete("/", DeletePostAsync);

        api.MapGet("/{id}", GetPostByIdAsync);
        api.MapGet("/get-by-slug/{slug}", GetPostBySlugAsync);
        api.MapPost("/paging", GetsPostAsync);

        api.MapGet("/home", GetsHomePostAsync);

        return api;
    }

    public static RouteGroupBuilder MapCommentsApiV1(this IEndpointRouteBuilder app)
    {
        var api = app.MapGroup("api/comment").HasApiVersion(1.0);

        api.MapPost("/", CommentAsync);
        //api.MapGet("/{postId}", GetsCommentByPostAsync);
        //api.MapGet("/comment-by-slug/{slug}", GetsCommentBySlugAsync);

        return api;
    }

    #region User endpoint

    public static async Task<Results<Ok, BadRequest<string>>> CreateUserAsync(
    [FromHeader(Name = "x-requestid")] Guid requestId,
    CreateUserRequest request,
    [AsParameters] PostServices services)
    {
        services.Logger.LogInformation(
            "Sending command: {CommandName} - {IdProperty}: {CommandId}",
            request.GetGenericTypeName(),
            nameof(request.UserName),
            request.UserName);


        var command = new CreateUserCommand
        {
            FullName = request.FullName,
            Username = request.UserName,
            Email = request.Email,
            Password = request.Password,
            PhoneNumber = request.PhoneNumber,
        };

        var result = await services.Mediator.Send(command);

        if (result)
        {
            services.Logger.LogInformation("CreateUserCommand succeeded - RequestId: {RequestId}", requestId);
        }
        else
        {
            services.Logger.LogWarning("CreateUserCommand failed - RequestId: {RequestId}", requestId);
        }

        return TypedResults.Ok();
    }

    #endregion

    #region Category endpoint

    public static async Task<Ok<ApiSuccessResponse>> GetsCategoryAsync([AsParameters] PostServices services, int size)
    {
        var query = new GetsCategoryQuery { Size = size };
        var result = await services.Mediator.Send(query);

        return TypedResults.Ok(new ApiSuccessResponse { Data = result });
    }

    public static async Task<Ok<ApiSuccessResponse>> CreateCategoryAsync(
    [FromForm] CreateCategoryCommand request,
    [AsParameters] PostServices services)
    {
        var result = await services.Mediator.Send(request);
        return TypedResults.Ok(new ApiSuccessResponse { Data = result, Message = "Tạo bài viết thành công" });
    }


    #endregion

    #region Post endpoint

    public static async Task<Results<Ok<ApiResponse>, BadRequest<ApiResponse>>> CreatePostAsync(
    [FromHeader(Name = "x-requestid")] Guid requestId,
    [FromForm] CreatePostCommand request,
    [AsParameters] PostServices services)
    {
        services.Logger.LogInformation(
            "Sending command: {CommandName} - {IdProperty}: {CommandId}",
            request.GetGenericTypeName(),
            nameof(request.Title),
            request.Title);

        try
        {
            var result = await services.Mediator.Send(request);

            services.Logger.LogInformation("CreatePostCommand succeeded - RequestId: {RequestId}", requestId);
            var successResponse = new ApiResponse
            {
                Message = "Tạo mới thành công",
                Data = "Dữ liệu đã được tạo thành công."
            };

            return TypedResults.Ok(successResponse);
        }
        catch (Exception ex)
        {
            services.Logger.LogError(ex, "An error occurred while processing the CreatePostCommand - RequestId: {RequestId}", requestId);

            var errorResponse = new ApiResponse
            {
                Message = "Lỗi hệ thống",
                DeveloperMessage = ex.Message
            };

            return TypedResults.BadRequest(errorResponse);
        }
    }


    public static async Task<Results<Ok<ApiResponse>, BadRequest<ApiResponse>>> UpdatePostAsync(
    [FromHeader(Name = "x-requestid")] Guid requestId,
    [FromForm] UpdatePostCommand request,
    [AsParameters] PostServices services)
    {
        services.Logger.LogInformation(
            "Sending command: {CommandName} - {IdProperty}: {CommandId}",
            request.GetGenericTypeName(),
            nameof(request.Title),
            request.Title);

        await services.Mediator.Send(request);

        var successResponse = new ApiResponse
        {
            Message = "Update thành công",
            Data = "Dữ liệu đã được update thành công."
        };

        return TypedResults.Ok(successResponse);
    }



    public static async Task<Ok<ApiSuccessResponse>> GetPostByIdAsync([AsParameters] PostServices services, string id)
    {
        var result = await services.PostQueries.FindByIdAsync(id);
        return TypedResults.Ok(new ApiSuccessResponse { Data = result });
    }

    public static async Task<Ok<ApiSuccessResponse>> GetPostBySlugAsync([AsParameters] PostServices services, string slug, bool? userWatch)
    {
        var result = await services.PostQueries.FindBySlugAsync(slug, userWatch);
        return TypedResults.Ok(new ApiSuccessResponse { Data = result });
    }

    public static async Task<Ok<ApiSuccessResponse>> GetsPostAsync(
        [AsParameters] PaginationRequest paginationRequest,
        [AsParameters] PostServices services,
        [FromBody] SearchPostRequest request)
    {
        var result = await services.PostQueries.GetPostsAsync(paginationRequest, request);
        return TypedResults.Ok(new ApiSuccessResponse { Data = result });
    }

    public static async Task<Ok<ApiSuccessResponse>> GetsHomePostAsync(
        [AsParameters] PostServices services)
    {
        var result = await services.PostQueries.GetHomePostAsync();
        return TypedResults.Ok(new ApiSuccessResponse { Data = result });
    }


    public static async Task<Ok<ApiSuccessResponse>> DeletePostAsync(
        [AsParameters] PostServices services,
        [FromQuery] string id)
    {
        var command = new DeletePostCommand { Id = id };

        var result = await services.Mediator.Send(command);
        return TypedResults.Ok(new ApiSuccessResponse { Data = result, Message = "Xóa bài viết thành công" });
    }

    private static string ShortenDescription(string description, int maxLength)
    {

        if (description.Length <= maxLength)
        {
            return description;
        }

        string shortened = description[..maxLength];
        return shortened + "...";
    }

    #endregion

    #region Comment endpoint

    public static async Task<Results<Ok, BadRequest<string>>> CommentAsync(
    [FromHeader(Name = "x-requestid")] Guid requestId,
    [FromBody] CommentCommand request,
    [AsParameters] PostServices services)
    {
        services.Logger.LogInformation(
            "Sending command: {CommandName} - {IdProperty}: {CommandId}",
            request.GetGenericTypeName(),
            nameof(request.Username),
            request.Username);

        var result = await services.Mediator.Send(request);

        if (result)
        {
            services.Logger.LogInformation("CommentCommand succeeded - RequestId: {RequestId}", requestId);
        }
        else
        {
            services.Logger.LogWarning("CommentCommand failed - RequestId: {RequestId}", requestId);
        }

        return TypedResults.Ok();
    }


    //public static async Task<Ok<PaginatedItems<Domain.AggregatesModel.CommentAggregate.Comment>>> GetsCommentByPostAsync(
    //    [AsParameters] PaginationRequest paginationRequest,
    //    [AsParameters] PostServices services,
    //    Guid postId)
    //{
    //    var pageSize = paginationRequest.PageSize;
    //    var pageIndex = paginationRequest.PageIndex;
    //    var offSet = pageIndex * pageSize - pageSize;

    //    var totalItems = await services.Context.Comments
    //        .Where(c => c.PostId == postId)
    //        .LongCountAsync();

    //    var itemsOnPage = await services.Context.Comments
    //        .Where(c => c.PostId == postId).OrderByDescending(x => x.CreatedDate)
    //        .Skip(offSet)
    //        .Take(pageSize)
    //        .ToListAsync();
    //    return TypedResults.Ok(new PaginatedItems<Domain.AggregatesModel.CommentAggregate.Comment>(pageIndex, pageSize, totalItems, itemsOnPage));
    //}

    //public static async Task<Ok<PaginatedItems<CommentDto>>> GetsCommentBySlugAsync(
    //    [AsParameters] PaginationRequest paginationRequest,
    //    [AsParameters] PostServices services,
    //    string slug)
    //{
    //    var pageSize = paginationRequest.PageSize;
    //    var pageIndex = paginationRequest.PageIndex;
    //    var offSet = pageIndex * pageSize - pageSize;

    //    var postId = services.Context.Posts.FirstOrDefault(x => x.Slug == slug)?.Id;

    //    var totalItems = await services.Context.Comments
    //        .Where(c => c.PostId == postId)
    //        .LongCountAsync();

    //    var itemsOnPage = await services.Context.Comments
    //        .Where(c => c.PostId == postId).OrderByDescending(x => x.CreatedDate)
    //        .Skip(offSet)
    //        .Take(pageSize)
    //        .Select(x => new CommentDto
    //        {
    //            Id = x.Id,
    //            Content = x.Content,
    //            Username = services.Context.Users.FirstOrDefault(u => u.Id == x.UserId).UserName,
    //            CreatedDate = x.CreatedDate.Value,
    //        })
    //        .ToListAsync();


    //    return TypedResults.Ok(new PaginatedItems<CommentDto>(pageIndex, pageSize, totalItems, itemsOnPage));
    //}

    #endregion
}

public record CreateUserRequest(
    string FullName,
    string UserName,
    string Email,
    string Password,
    string PhoneNumber,
    UserType UserType
);

public class SearchPostRequest
{
    public List<string> CategoryIds { get; set; } = [];
    public string Title { get; set; }
    public string Slug { get; set; }
}



public class CategoryDto
{
    public string Id { get; set; }
    public string Title { get; set; }
    public string TagName { get; set; }
}

public class CommentDto
{
    public string Id { get; set; }
    public string Content { get; set; }
    public string Username { get; set; }
    public DateTime CreatedDate { get; set; }
    public string CreatedDateStr => CreatedDate.ConvertToAgoString();
}

/// <summary>
/// Danh sách bài viết hiển thị ở trang chủ
/// </summary>
public class HomePostDto
{
    public List<PostDto> LatestNews { get; set; } = [];
    public List<PostDto> LatestBlog { get; set; } = [];
    public List<PostDto> MostRead { get; set; } = [];

}

public class PostDto
{
    public string Id { get; set; }
    public string Title { get; set; }
    public string DescriptionShort { get; set; }
    public string ImageUrl { get; set; }
    public string Slug { get; set; }
    public DateTime? CreatedDate { get; set; }
    public int? CountWatch { get; set; }
    public List<CategoryDto> Categories { get; set; } = [];
}
