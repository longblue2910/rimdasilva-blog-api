using Contracts.Common.Responses;
using Contracts.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace Core.Attributes
{
    public class HttpGlobalExceptionFilter : IExceptionFilter
    {
        /// <summary>
        /// System exception
        /// </summary>
        /// <param name="context"></param>
        public void OnException(ExceptionContext context)
        {
            var exception = context.Exception;
            var developerMessage = exception.Message + "\r\n" + exception.StackTrace;

            string innerEx500 = exception.Message;

            while (exception.InnerException != null)
            {
                developerMessage += "\r\n--------------------------------------------------\r\n";
                exception = exception.InnerException;
                developerMessage += (exception.Message + "\r\n" + exception.StackTrace);

                innerEx500 += $" | {exception.Message}";
            }

            if (context.ModelState.ErrorCount > 0)
            {
                var errors = context.ModelState.Where(v => v.Value.Errors.Count > 0)
                    .ToDictionary(
                        kvp => $"{char.ToLower(kvp.Key[0])}{kvp.Key[1..]}",
                        kvp => kvp.Value.Errors.FirstOrDefault()?.ErrorMessage
                    );

                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.UnprocessableEntity;
                context.Result = new UnprocessableEntityObjectResult(new ApiResponse
                {
                    StatusCode = StatusCodes.Status422UnprocessableEntity,
                    //Message = developerMessage,
                    Data = errors
                });
                context.ExceptionHandled = true;
                return;
            }

            var json = new ApiResponse
            {
                Message = context.Exception.Message,
                DeveloperMessage = developerMessage
            };

            var userName = context.HttpContext.User.Identity.IsAuthenticated
                ? context.HttpContext.User.Identity.Name : "Guest"; //Gets user Name from user Identity 

            // 400 Bad Request
            if (context.Exception.GetType() == typeof(BadRequestException))
            {
                var errorCode = (int?)exception.Data[BadRequestException.ErrorCode];
                if (errorCode != null)
                {
                    json.StatusCode = errorCode.Value;
                    context.HttpContext.Response.StatusCode = errorCode.Value;
                }
                else
                {
                    json.StatusCode = StatusCodes.Status400BadRequest;
                    context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    json.IsSuccess = false;
                }
                context.Result = new BadRequestObjectResult(json);
            }
            // 401 UnAuthorization
            else if (context.Exception.GetType() == typeof(UnauthorizedException))
            {
                var errorCode = (int?)exception.Data[UnauthorizedException.ErrorCode];
                if (errorCode != null)
                {
                    json.StatusCode = errorCode.Value;
                    context.HttpContext.Response.StatusCode = errorCode.Value;
                }
                else
                {
                    json.StatusCode = StatusCodes.Status401Unauthorized;
                    context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    json.IsSuccess = false;
                }
                context.Result = new BadRequestObjectResult(json);
            }
            // 404 Not Found
            else if (context.Exception.GetType() == typeof(NotFoundException))
            {
                json.StatusCode = StatusCodes.Status404NotFound;
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;
                context.Result = new NotFoundObjectResult(json);
                json.IsSuccess = false;

            }
            // 500 Internal Server Error
            else
            {
                json.IsSuccess = false;
                json.Message = $"Lỗi hệ thống, {innerEx500}";
                json.StatusCode = StatusCodes.Status500InternalServerError;
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Result = new InternalServerErrorObjectResult(json);
            }
            context.ExceptionHandled = true;
        }
    }
}
