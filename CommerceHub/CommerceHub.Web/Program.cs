
//.net core, web sunucusu olarak platform bağımsız Kestrel'i yazdılar!!!!!
//İstek gelir -> Kestrel dinler -> HttpContext nesnesi oluşturur  -> Geri kalanı backend'in işidir.
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

var app = builder.Build();

app.Use(async (context, next) =>
{

    Console.WriteLine($"[İSTEK GELDİ] -> {context.Request.Method} - {context.Request.Path}");
    await next();
    Console.WriteLine($"[YANIT ÜRETİLDİ] -> {context.Response.StatusCode}");
    

});


app.MapControllers();
app.Run();


