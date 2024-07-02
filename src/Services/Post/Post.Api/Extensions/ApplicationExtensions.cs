using Post.Api.Apis;

namespace Post.Api.Extensions;

public static class ApplicationExtensions
{
    public static void ApiManage(this WebApplication app)
    {
        app.NewVersionedApi("User")
           .MapUsersApiV1();

        app.NewVersionedApi("Category")
           .MapCategoriesApiV1();

        app.NewVersionedApi("Post")
            .MapPostsApiV1().RequireAuthorization("WriteScopePolicy").RequireAuthorization("ReadScope");

        app.NewVersionedApi("Comment")
            .MapCommentsApiV1();
    }
}
