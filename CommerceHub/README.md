# CommerceHub — .NET 10 Eğitim Projesi

Bu proje, Halkbank 2026 .NET Eğitimi kapsamında **ASP.NET Core** ve **.NET 10** teknolojilerini öğrenmek amacıyla oluşturulmuş örnek bir ticaret uygulamasıdır.

---

## 📚 Öğrenilen Konular

### 1. ASP.NET Core Minimal API
- `WebApplication.CreateBuilder` ile uygulama oluşturma
- `app.MapGet` ile endpoint tanımlama
- `Results.Ok(...)` gibi yardımcı metodlarla HTTP yanıtı üretme

```csharp
app.MapGet("/", () => Results.Ok("istek, endpoint'e ulaştı!"));
```

---

### 2. Kestrel Web Sunucusu
- .NET Core'un platform bağımsız yerleşik web sunucusu **Kestrel**
- `appsettings.json` üzerinden port ve limit yapılandırması
- İstek akışı: **İstek → Kestrel → HttpContext → Backend**

```json
"Kestrel": {
  "Endpoints": {
    "Http": { "Url": "http://localhost:5000" }
  },
  "Limits": {
    "MaxRequestBodySize": "1048576"
  }
}
```

---

### 3. Middleware Pipeline
- `app.Use(...)` ile anonim middleware yazımı
- `RequestDelegate` ve `HttpContext` kavramları
- Middleware'lerin **sıralı zincir** yapısı (pipeline)
- `app.UseMiddleware<T>()` ile sınıf tabanlı middleware kaydı

**Örnek Middleware Akışı:**
```
İstek → ExceptionHandlingMiddleware → RequestTimingMiddleware → Endpoint → Yanıt
```

---

### 4. Özel Middleware Sınıfları

#### `RequestTimingMiddleware`
- `Stopwatch` ile istek süresini ölçer
- `ILogger<T>` ile süre bilgisini loglar
- `try / catch / finally` bloğu ile hata durumunda bile süre kaydeder

```csharp
logger.LogInformation($"[TIMING] ({status}) -> {context.Request.Path} ... {elapsed} ms sürdü.");
```

#### `ExceptionHandlingMiddleware`
- Merkezi hata yönetimi sağlar
- `switch expression` (pattern matching) ile exception türüne göre HTTP status kodu belirler
- `NotFoundException` → **404**, `ValidationException` → **500**
- Yanıt gövdesini JSON formatında döner

```csharp
var (statusCode, message) = ex switch
{
    NotFoundException => (StatusCodes.Status404NotFound, ex.Message),
    ValidationException => (StatusCodes.Status500InternalServerError, ex.Message),
    _ => (StatusCodes.Status500InternalServerError, "Bilinmeyen bir hata oluştu")
};
```

---

### 5. Özel Exception Sınıfları
- `NotFoundException` ile anlamlı, iş kuralı odaklı hata fırlatma
- Exception sınıflarının merkezi middleware ile yakalanması

---

### 6. Configuration & Options Pattern
- `appsettings.json` içinde uygulama ayarları tanımlama
- `IOptions<T>` ile strongly-typed konfigürasyon okuma
- `builder.Services.Configure<T>(...)` ile DI container'a kayıt

```csharp
builder.Services.Configure<CommerceSettings>(
    builder.Configuration.GetSection("CommerceSettings"));
```

```csharp
app.MapGet("/ayarlar", (IOptions<CommerceSettings> options) =>
{
    var settings = options.Value;
    return Results.Ok(new { settings.DefaultCurrency, settings.MaxOrderItemCount });
});
```

---

### 7. Dependency Injection (DI)
- Constructor injection ile `RequestDelegate` ve `ILogger<T>` bağımlılıklarının çözülmesi
- Middleware bağımlılıklarının **Singleton** yaşam döngüsüne sahip olması

---

### 8. Logging
- `ILogger<T>` arayüzü ile yapılandırılmış loglama
- `appsettings.json` üzerinden log seviyesi yapılandırması (`Information`, `Warning`)

---

## 🗂 Proje Yapısı

```
CommerceHub.Web/
├── Exceptions/
│   └── NotFoundException.cs          # Özel exception sınıfı
├── Middleware/
│   ├── ExceptionHandlingMiddleware.cs # Merkezi hata yönetimi
│   └── RequestTiminingMiddleware.cs   # İstek süre ölçümü
├── Settings/
│   └── CommerceSettings.cs           # Options pattern modeli
├── appsettings.json                   # Kestrel, logging, uygulama ayarları
└── Program.cs                         # Uygulama giriş noktası
```

---

## 🚀 Çalıştırma

```bash
dotnet run --project CommerceHub.Web
```

Uygulama varsayılan olarak `http://localhost:5000` adresinde çalışır.

| Endpoint   | Açıklama                              |
|------------|---------------------------------------|
| `GET /`    | Temel endpoint yanıtı                 |
| `GET /hata`| `NotFoundException` fırlatır (test)   |
| `GET /ayarlar` | `CommerceSettings` değerlerini döner |

---

## 🛠 Kullanılan Teknolojiler

- **.NET 10**
- **ASP.NET Core Minimal API**
- **Kestrel**
- **Microsoft.Extensions.Options**
- **Microsoft.Extensions.Logging**
