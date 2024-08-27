using Newtonsoft.Json;

namespace Backend.Middleware
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            if (context.Response.StatusCode == StatusCodes.Status403Forbidden)
            {
                context.Response.ContentType = "application/json";
                var result = JsonConvert.SerializeObject(new
                {
                    status = "Error",
                    message = "Access denied. You do not have the required role to perform this action."
                });
                return context.Response.WriteAsync(result);
            }

            throw exception;
        }
    }
}
