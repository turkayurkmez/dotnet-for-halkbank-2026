
//.net core, web sunucusu olarak platform bağımsız Kestrel'i yazdılar!!!!!
//İstek gelir -> Kestrel dinler -> HttpContext nesnesi oluşturur  -> Geri kalanı backend'in işidir.
var builder = WebApplication.CreateBuilder(args);


