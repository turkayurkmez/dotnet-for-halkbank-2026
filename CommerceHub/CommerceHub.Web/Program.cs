
//.net core, web sunucusu olarak platform bağımsız Kestrel'i yazdılar!!!!!
//İstek gelir -> Kestrel dinler -> HttpContext nesnesi oluşturur  -> Geri kalanı backend'in işidir.
using CommerceHub.Web.Exceptions;
using CommerceHub.Web.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

var app = builder.Build();

//app.Use(async (context, next) =>
//{

//    Console.WriteLine($"[İSTEK GELDİ] -> {context.Request.Method} - {context.Request.Path}");
//    await next();
//    Console.WriteLine($"[YANIT ÜRETİLDİ] -> {context.Response.StatusCode}");


//});

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseMiddleware<RequestTiminingMiddleware>();

//app.UseRouting();
//app.UseAuthentication();//kim? nereye?


app.MapGet("/", () => Results.Ok("istek, endpoint'e ulaştı!"));
app.MapGet("/hata", () =>
{

    throw new NotFoundException("Test verisi bulunamadı!");
}); 
app.MapControllers();
app.Run();


