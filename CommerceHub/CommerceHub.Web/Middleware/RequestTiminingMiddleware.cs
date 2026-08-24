using System.Diagnostics;

namespace CommerceHub.Web.Middleware
{
    public class RequestTiminingMiddleware
    {
        //kendisinden SONRA başka bir middleware olabilir.
        //Middleware'in bağımlılıkları (dependency) her zaman Singleton'dur.
        private readonly RequestDelegate next;
        public RequestTiminingMiddleware(RequestDelegate next)
        {
            this.next = next;
        }
        //peki, middleware'iniz ne yapacak?
        public async Task InvokeAsync(HttpContext context, ILogger<RequestTiminingMiddleware> logger)
        {
            //işlemi yap..
            var stopwatch = Stopwatch.StartNew();
            //bir sonraki middleware'e git
            await next(context);
            stopwatch.Stop();
            var elapsed = stopwatch.ElapsedMilliseconds;
            logger.LogInformation($"[TIMING] -> {context.Request.Path} adresine gelen {context.Request.Method} isteği, {elapsed} ms sürdü.");
            //bir sonraki bittikten sonra gerekirse işleme devam et.
        }
    }
}
