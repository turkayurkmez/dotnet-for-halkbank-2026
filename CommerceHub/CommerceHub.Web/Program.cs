
//.net core, web sunucusu olarak platform bağımsız Kestrel'i yazdılar!!!!!
//İstek gelir -> Kestrel dinler -> HttpContext nesnesi oluşturur  -> Geri kalanı backend'in işidir.
using CommerceHub.Web.Exceptions;
using CommerceHub.Web.Middleware;
using CommerceHub.Web.Services;
using CommerceHub.Web.Settings;
using Microsoft.Extensions.Options;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
//Binding (IOptions Binding) : 
builder.Services.Configure<CommerceSettings>(builder.Configuration.GetSection("CommerceSettings"));

builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<NotificationService>();

var app = builder.Build();

//var value = app.Configuration["CommerceSettings:DefaultCurrency"];
//Console.WriteLine($"DİKKAT DEĞER: {value}");



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
app.MapGet("/ayarlar", (IOptions<CommerceSettings> options) =>
{
    //TODO 1: Burada, IOptions test edilecek.
    var settings = options.Value;
    return Results.Ok(new
    {
        settings.DefaultCurrency,
        settings.MaxOrderItemCount

    });
});
app.MapControllers();
app.Run();


