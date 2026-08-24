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
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;

            await context.Response.WriteAsJsonAsync(new { error = "Bilinmeyen bir hata oluştu!" });

           
        }
    }
}
