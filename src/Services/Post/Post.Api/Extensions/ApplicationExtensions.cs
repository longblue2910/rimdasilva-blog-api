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
            .MapPostsApiV1();

        app.NewVersionedApi("Comment")
            .MapCommentsApiV1();
    }
}
