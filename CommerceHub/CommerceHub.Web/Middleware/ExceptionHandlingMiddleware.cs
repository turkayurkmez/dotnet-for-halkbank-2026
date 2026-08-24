using CommerceHub.Web.Exceptions;

namespace CommerceHub.Web.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, ILogger<ExceptionHandlingMiddleware> logger)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await handleExceptionAsync(context, ex,logger);  
            }
        }

        private async Task handleExceptionAsync(HttpContext context, Exception ex, ILogger<ExceptionHandlingMiddleware> logger)
        {
            logger.LogError($"[!!HATA!!] -> {ex.GetType().Name}: {ex.Message}");

            var (statusCode, message) = ex switch
            {
                NotFoundException => (StatusCodes.Status404NotFound, ex.Message),
                ValidationException => (StatusCodes.Status500InternalServerError,ex.Message),
                _ => (StatusCodes.Status500InternalServerError,"Bilinmeyen bir hata oluştu")
            };

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;

            await context.Response.WriteAsJsonAsync(new { error = message });

           
        }
    }
}
