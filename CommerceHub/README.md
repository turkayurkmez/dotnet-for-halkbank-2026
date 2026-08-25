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
- `builder.Services.AddScoped<T>()` ile servislerin DI container'a kaydedilmesi
- Controller'larda constructor injection ile servis kullanımı (`ProductService`)

```csharp
builder.Services.AddScoped<ProductService>();
```

```csharp
public ProductsController(ProductService productService)
{
    _productService = productService;
}
```

---

### 8. Single Responsibility Principle (SRP) & Servis Katmanı Ayrımı
- Büyük bir sınıfın sorumluluklarının **küçük, odaklı sınıflara** bölünmesi
- `ProductService` → iş mantığı koordinasyonu
- `ProductRepository` → veri erişimi (in-memory liste)
- `ProductPriceCalculator` → fiyat hesaplama algoritması
- `NotificationService` → bildirim gönderim koordinasyonu (eski: `EmailSender`)

```
ProductService
├── ProductRepository       → veriyi getirir
├── ProductPriceCalculator  → fiyatı hesaplar
└── NotificationService     → bildirim kanallarını koordine eder
```

---

### 9. Open/Closed Principle (OCP) & Interface Kullanımı
- **Bir sınıf gelişime açık, değişime kapalı olmalıdır**
- `INotification` arayüzü ile bildirim kanalları soyutlandı
- Her kanal kendi sınıfında implement edildi: `EmailNotification`, `SMSNotification`, `WhatsAppNotification`
- `NotificationService.Notify(INotification, string)` metodu, hangi kanalın kullanıldığını bilmez; sadece `Send()` eylemini çağırır
- Yeni bir bildirim kanalı eklemek için **mevcut kodu değiştirmeye gerek yoktur**, yeni bir sınıf eklemek yeterlidir

```csharp
public interface INotification
{
    void Send(string message);
}

// Yeni kanal eklemek: mevcut kodu değiştirmeden yeni sınıf yaz
public class WhatsAppNotification : INotification
{
    public void Send(string message) => Console.WriteLine($"Whatsapp: {message}");
}
```

```csharp
// NotificationService hangi kanalın kullanıldığını bilmiyor, sadece eylemi (Send) biliyor:
public void Notify(INotification notification, string message)
{
    notification.Send(message);
}
```

> **OCP ihlali örneği (kaçınılan pattern):** `switch/case` ile her yeni kanal için mevcut kodu değiştirmek zorunda kalmak.

---

### 9. MVC Controller Yapısı
- `ControllerBase` türetmesi ile API controller oluşturma
- `[ApiController]` ve `[Route]` attribute'ları
- `[HttpGet]`, `[HttpGet("{id:int}")]` ile route tanımlama
- `IActionResult` / `Ok(...)` ile HTTP yanıtı döndürme

```csharp
[Route("api/[controller]")]
[ApiController]
public class ProductsController : ControllerBase { ... }
```

---

### 10. Logging
- `ILogger<T>` arayüzü ile yapılandırılmış loglama
- `LogWarning` ile ürün bulunamadığında uyarı loglama
- `LogInformation` ile hesaplanan fiyatı loglama
- `appsettings.json` üzerinden log seviyesi yapılandırması (`Information`, `Warning`)

---

## 🗂 Proje Yapısı

```
CommerceHub.Web/
├── Controllers/
│   └── ProductsController.cs          # MVC API controller
├── Exceptions/
│   └── NotFoundException.cs           # Özel exception sınıfı
├── Middleware/
│   ├── ExceptionHandlingMiddleware.cs  # Merkezi hata yönetimi
│   └── RequestTiminingMiddleware.cs    # İstek süre ölçümü
├── Models/
│   └── Product.cs                     # Ürün modeli
├── Services/
│   ├── ProductService.cs              # İş mantığı koordinasyonu
│   ├── ProductRepository.cs           # Veri erişim katmanı
│   ├── ProductPriceCalculator.cs      # Fiyat hesaplama
│   ├── NotificationService.cs         # Bildirim gönderim koordinasyonu (OCP)
│   ├── INotification.cs               # Bildirim arayüzü + Email/SMS/WhatsApp implementasyonları
│   └── EmailSender.cs                 # (Eski) e-posta gönderici
├── Settings/
│   └── CommerceSettings.cs            # Options pattern modeli
├── appsettings.json                    # Kestrel, logging, uygulama ayarları
└── Program.cs                          # Uygulama giriş noktası
```

---

## 🚀 Çalıştırma

```bash
dotnet run --project CommerceHub.Web
```

Uygulama varsayılan olarak `http://localhost:5000` adresinde çalışır.

| Endpoint                     | Açıklama                                         |
|------------------------------|--------------------------------------------------|
| `GET /`                      | Temel endpoint yanıtı                            |
| `GET /hata`                  | `NotFoundException` fırlatır (test)              |
| `GET /ayarlar`               | `CommerceSettings` değerlerini döner             |
| `GET /api/products`          | Tüm ürünleri indirimli fiyatlarıyla listeler     |
| `GET /api/products/{id}`     | Belirli bir ürünün hesaplanmış fiyatını döner    |

---

## 🛠 Kullanılan Teknolojiler

- **.NET 10**
- **ASP.NET Core Minimal API & MVC**
- **Kestrel**
- **Microsoft.Extensions.Options**
- **Microsoft.Extensions.Logging**
- **Microsoft.Extensions.DependencyInjection**
