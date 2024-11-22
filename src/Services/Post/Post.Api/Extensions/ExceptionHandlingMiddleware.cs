using Contracts.Common.Responses;
using Contracts.Exceptions;

namespace Post.Api.Extensions
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                // Tiếp tục chuỗi middleware nếu không có lỗi
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                // Xử lý ngoại lệ khi xảy ra
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            // Tạo ApiResponse để trả về thông tin lỗi
            var response = new ApiResponse
            {
                Message = exception.Message,
                DeveloperMessage = exception.StackTrace,
            };

            // Xác định mã trạng thái HTTP dựa trên exception
            if (exception is BadRequestException)
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
            }
            else if (exception is UnauthorizedException)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            }
            else if (exception is NotFoundException)
            {
                context.Response.StatusCode = StatusCodes.Status404NotFound;
            }
            else
            {
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            }

            // Trả về ApiResponse dưới dạng JSON
            return context.Response.WriteAsJsonAsync(response);
        }
    }

}
