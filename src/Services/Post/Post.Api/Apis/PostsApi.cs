using Infrastructure.Extensions;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Post.Api.Applications.Commands.Comment;
using Post.Api.Applications.Commands.Post;
using Post.Api.Applications.Commands.User;
using Post.Api.Applications.Queries.Category;
using Post.Api.Models;
using Post.Domain.AggregatesModel.CategoryAggregate;
using Post.Domain.AggregatesModel.UserAggregate;

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
        return api;
    }

    public static RouteGroupBuilder MapPostsApiV1(this IEndpointRouteBuilder app)
    {
        var api = app.MapGroup("api/post").HasApiVersion(1.0);

        api.MapPost("/", CreatePostAsync).DisableAntiforgery();
        api.MapPut("/", UpdatePostAsync).DisableAntiforgery();

        api.MapGet("/{id}", GetPostByIdAsync);
        api.MapGet("/get-by-slug/{slug}", GetPostBySlugAsync);
        api.MapPost("/paging", GetsPostAsync);

        return api;
    }

    public static RouteGroupBuilder MapCommentsApiV1(this IEndpointRouteBuilder app)
    {
        var api = app.MapGroup("api/comment").HasApiVersion(1.0);

        api.MapPost("/", CommentAsync);
        api.MapGet("/{postId}", GetsCommentByPostAsync);
        api.MapGet("/comment-by-slug/{slug}", GetsCommentBySlugAsync);

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

    public static async Task<Ok<List<Category>>> GetsCategoryAsync([AsParameters] PostServices services, int size)
    {
        var query = new GetsCategoryQuery { Size = size };
        var result = await services.Mediator.Send(query);

        return TypedResults.Ok(result);
    }

    #endregion

    #region Post endpoint

    public static async Task<Results<Ok, BadRequest<string>>> CreatePostAsync(
    [FromHeader(Name = "x-requestid")] Guid requestId,
    [FromForm] CreatePostCommand request,
    [AsParameters] PostServices services)
    {
        services.Logger.LogInformation(
            "Sending command: {CommandName} - {IdProperty}: {CommandId}",
            request.GetGenericTypeName(),
            nameof(request.Title),
            request.Title);

        var result = await services.Mediator.Send(request);

        if (result)
        {
            services.Logger.LogInformation("CreatePostCommand succeeded - RequestId: {RequestId}", requestId);
        }
        else
        {
            services.Logger.LogWarning("CreatePostCommand failed - RequestId: {RequestId}", requestId);
        }

        return TypedResults.Ok();
    }

    public static async Task<Results<Ok, BadRequest<string>>> UpdatePostAsync(
    [FromHeader(Name = "x-requestid")] Guid requestId,
    [FromForm] UpdatePostCommand request,
    [AsParameters] PostServices services)
    {
        services.Logger.LogInformation(
            "Sending command: {CommandName} - {IdProperty}: {CommandId}",
            request.GetGenericTypeName(),
            nameof(request.Title),
            request.Title);

        var result = await services.Mediator.Send(request);

        if (result)
        {
            services.Logger.LogInformation("UpdatePostCommand succeeded - RequestId: {RequestId}", requestId);
        }
        else
        {
            services.Logger.LogWarning("UpdatePostCommand failed - RequestId: {RequestId}", requestId);
        }

        return TypedResults.Ok();
    }



    public static async Task<Ok<Domain.AggregatesModel.PostAggregate.Post>> GetPostByIdAsync([AsParameters] PostServices services, string id)
    {
        var result = await services.PostQueries.FindByIdAsync(id);
        return TypedResults.Ok(result);
    }

    public static async Task<Ok<Domain.AggregatesModel.PostAggregate.Post>> GetPostBySlugAsync([AsParameters] PostServices services, string slug)
    {
        var result = await services.PostQueries.FindBySlugAsync(slug);
        return TypedResults.Ok(result);
    }

    public static async Task<Ok<PaginatedItems<PostDto>>> GetsPostAsync(
        [AsParameters] PaginationRequest paginationRequest,
        [AsParameters] PostServices services,
        [FromBody] SearchPostRequest request)
    {
        var pageSize = paginationRequest.PageSize;
        var pageIndex = paginationRequest.PageIndex;
        var offSet = pageIndex * pageSize - pageSize;

        var totalItems = await services.Context.Posts
            .Where
            (
               c => (!request.CategoryId.HasValue || c.Categories.Any(x => (x.Id == request.CategoryId))) &&
                    (string.IsNullOrEmpty(request.Slug) || c.Categories.Any(x => (x.Slug.StartsWith(request.Slug)))) &&
                    (string.IsNullOrEmpty(request.Title) || c.Title.StartsWith(request.Title))
            )
            .LongCountAsync();

        var itemsOnPage = await services.Context.Posts
            .Where
            (
               c => (c.Categories.Any(x => (!request.CategoryId.HasValue || x.Id == request.CategoryId))) &&
                    (string.IsNullOrEmpty(request.Slug) || c.Categories.Any(x => (x.Slug.StartsWith(request.Slug)))) &&
                    (string.IsNullOrEmpty(request.Title) || c.Title.StartsWith(request.Title))
            ).Skip(offSet)
            .Take(pageSize)
            .Include(x => x.Categories)
            .Select(x => new PostDto
            {
                Slug = x.Slug,
                Title = x.Title,
                Description = x.Description,
                ImageUrl = x.ImageUrl,
                Id = x.Id,
                CreatedDate = x.CreatedDate,
                Categories = x.Categories.Select(c => new CategoryDto
                {
                    Id = c.Id,
                    TagName = c.TagName,
                    Title = c.Title,
                }).ToList(),
            })
            .ToListAsync();

        foreach (var item in itemsOnPage)
        {
            item.Description = ShortenDescription(item.Description, 200);
        }

        return TypedResults.Ok(new PaginatedItems<PostDto>(pageIndex, pageSize, totalItems, itemsOnPage));
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


    public static async Task<Ok<PaginatedItems<Domain.AggregatesModel.CommentAggregate.Comment>>> GetsCommentByPostAsync(
        [AsParameters] PaginationRequest paginationRequest,
        [AsParameters] PostServices services,
        Guid postId)
    {
        var pageSize = paginationRequest.PageSize;
        var pageIndex = paginationRequest.PageIndex;
        var offSet = pageIndex * pageSize - pageSize;

        var totalItems = await services.Context.Comments
            .Where(c => c.PostId == postId)
            .LongCountAsync();

        var itemsOnPage = await services.Context.Comments
            .Where(c => c.PostId == postId).OrderByDescending(x => x.CreatedDate)
            .Skip(offSet)
            .Take(pageSize)
            .ToListAsync();
        return TypedResults.Ok(new PaginatedItems<Domain.AggregatesModel.CommentAggregate.Comment>(pageIndex, pageSize, totalItems, itemsOnPage));
    }

    public static async Task<Ok<PaginatedItems<CommentDto>>> GetsCommentBySlugAsync(
        [AsParameters] PaginationRequest paginationRequest,
        [AsParameters] PostServices services,
        string slug)
    {
        var pageSize = paginationRequest.PageSize;
        var pageIndex = paginationRequest.PageIndex;
        var offSet = pageIndex * pageSize - pageSize;

        var postId = services.Context.Posts.FirstOrDefault(x => x.Slug == slug)?.Id;

        var totalItems = await services.Context.Comments
            .Where(c => c.PostId == postId)
            .LongCountAsync();

        var itemsOnPage = await services.Context.Comments
            .Where(c => c.PostId == postId).OrderByDescending(x => x.CreatedDate)
            .Skip(offSet)
            .Take(pageSize)
            .Select(x => new CommentDto
            {
                Id = x.Id,
                Content = x.Content,
                Username = services.Context.Users.FirstOrDefault(u => u.Id == x.UserId).UserName,
                CreatedDate = x.CreatedDate,
            })
            .ToListAsync();
        return TypedResults.Ok(new PaginatedItems<CommentDto>(pageIndex, pageSize, totalItems, itemsOnPage));
    }

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


public record SearchPostRequest(
    Guid? CategoryId,
    string Title,
    string Slug
);

public class PostDto
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string ImageUrl { get; set; }
    public string Slug { get; set; }
    public DateTime? CreatedDate { get; set; }
    public List<CategoryDto> Categories { get; set; } = [];
}

public class CategoryDto
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string TagName { get; set; }
}

public class CommentDto
{
    public Guid Id { get; set; }
    public string Content { get; set; }
    public string Username { get; set; }
    public DateTime? CreatedDate { get; set; }
    public string CreatedDateStr => CreatedDate.HasValue ? CreatedDate?.ToString("dd-MM-yyyy") : null;
}