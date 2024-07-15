using Duende.IdentityServer.Models;

namespace Identity.Api;

public static class Config
{
    public static IEnumerable<ApiResource> GetApis()
    {
        return new List<ApiResource>
        {
            new("Posts", "Posts Service"),
        };
    }

    // ApiScope is used to protect the API 
    //The effect is the same as that of API resources in IdentityServer 3.x
    public static IEnumerable<ApiScope> GetApiScopes()
    {
        return new List<ApiScope>
        {
            new("Posts", "Posts Service"),
        };
    }

    // Identity resources are data like user ID, name, or email address of a user
    // see: http://docs.identityserver.io/en/release/configuration/resources.html
    public static IEnumerable<IdentityResource> GetResources()
    {
        return new List<IdentityResource>
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile()
        };
    }

    // client want to access resources (aka scopes)
    public static IEnumerable<Client> GetClients(IConfiguration configuration)
    {
        return new List<Client>
        {
            new() {
                ClientId = "postswaggerui",
                ClientName = "Post Swagger UI",
                AllowedGrantTypes = GrantTypes.Implicit,
                AllowAccessTokensViaBrowser = true,

                RedirectUris = { $"{configuration["PostApiClient"]}/swagger/oauth2-redirect.html" },
                PostLogoutRedirectUris = { $"{configuration["PostApiClient"]}/swagger/" },

                AllowedScopes =
                {
                    "Posts"
                }
            }
        };
    }
}
