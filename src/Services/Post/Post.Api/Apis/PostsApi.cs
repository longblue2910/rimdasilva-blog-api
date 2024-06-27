using Infrastructure.Extensions;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Post.Api.Applications.Commands.Post;
using Post.Api.Applications.Commands.User;
using Post.Api.Applications.Queries.Category;
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

        api.MapPost("/", CreatePostAsync);
        api.MapGet("/{id}", GetPostByIdAsync);
        api.MapGet("/get-by-slug/{slug}", GetPostByIdAsync);

        return api;
    }


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



    public static async Task<Ok<List<Category>>> GetsCategoryAsync([AsParameters] PostServices services)
    {
        var query = new GetsCategoryQuery {};
        var result = await services.Mediator.Send(query);

        return TypedResults.Ok(result);
    }


    public static async Task<Results<Ok, BadRequest<string>>> CreatePostAsync(
    [FromHeader(Name = "x-requestid")] Guid requestId,
    CreatePostCommand request,
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

}

public record CreateUserRequest(
    string FullName,
    string UserName,
    string Email,
    string Password,
    string PhoneNumber,
    UserType UserType
);

