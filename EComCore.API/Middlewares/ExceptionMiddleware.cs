using EComCore.Application.Exceptions;
using System.Net;
using System.Text.Json;

namespace EComCore.API.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            HttpStatusCode statusCode = HttpStatusCode.InternalServerError;
            string message = "Internal Server Error";

            switch (exception)
            {
                case NotFoundException:
                    statusCode = HttpStatusCode.NotFound;
                    message = exception.Message;
                    break;

                case ValidationException:
                    statusCode = HttpStatusCode.BadRequest;
                    message = exception.Message;
                    break;

                default:
                    // log exception here (Serilog/NLog etc.)
                    break;
            }

            context.Response.StatusCode = (int)statusCode;
            var result = JsonSerializer.Serialize(new { error = message });
            return context.Response.WriteAsync(result);
        }
    }
}
