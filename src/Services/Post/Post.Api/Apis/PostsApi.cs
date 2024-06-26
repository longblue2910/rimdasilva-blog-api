using Infrastructure.Extensions;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Post.Api.Applications.Commands.User;

namespace Post.Api.Apis;

public static class PostsApi
{
    public static RouteGroupBuilder MapOrdersApiV1(this IEndpointRouteBuilder app)
    {
        var api = app.MapGroup("api/posts").HasApiVersion(1.0);
        api.MapPost("/", CreateUserAsync);

        return api;
    }

    public static RouteGroupBuilder MapOrdersApiV2(this IEndpointRouteBuilder app)
    {
        var api = app.MapGroup("api/posts").HasApiVersion(2.0);
        api.MapPost("/", CreateUserAsync);


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
            Phone = request.Phone,
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
}

public record CreateUserRequest(
    string FullName,
    string UserName,
    string Email,
    string Password,
    string Phone
   );
