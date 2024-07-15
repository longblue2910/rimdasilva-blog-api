using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Contracts.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]

    public class AuthorizationAttribute : ActionFilterAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            //Check enpoint AllowAnonymous
            var endpoint = context.ActionDescriptor.EndpointMetadata
             .OfType<AllowAnonymousAttribute>()
             .FirstOrDefault();

            if (endpoint == null)
            {
                // Endpoint không có thuộc tính AllowAnonymous, kiểm tra đăng nhập
                var userIdentity = context.HttpContext.User?.Identity;
                var userName = userIdentity?.Name;

                if (string.IsNullOrEmpty(userName))
                {
                    // Không đăng nhập
                    context.Result = new JsonResult(new { code = 401, message = "Unauthorized." });
                }
            }
        }
    }
}
