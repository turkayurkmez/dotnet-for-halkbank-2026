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

- Bildirim implementasyonları ayrı dosyalara taşındı: `EmailNotification.cs`, `SMSNotification.cs`, `WhatsAppNotification.cs`

---

### 10. Liskov Substitution Principle (LSP)
- **Alt sınıflar, üst sınıfların yerine sorunsuzca kullanılabilmelidir**
- `GiftOrder` sınıfı `Order`'dan türetildiğinde `GetTotal()` metodu `NotSupportedException` fırlatıyordu → **LSP ihlali**
- Çözüm: `GiftOrder`, `Order`'dan **miras almaktan çıkarıldı**; bunun yerine yalnızca fiyatlanabilir siparişler için `IPricableOrder` arayüzü tanımlandı
- `OrderService.PrintTotal(List<IPricableOrder>)` yalnızca fiyatlandırılabilen siparişlerle çalışır

```csharp
public interface IPricableOrder
{
    decimal GetTotal();
}

public class Order : IPricableOrder
{
    public List<decimal> ItemPrices { get; set; }
    public decimal GetTotal() => ItemPrices.Sum();
}

public class GiftOrder // Order'dan miras almıyor — LSP ihlali önlendi
{
    public string Note { get; set; }
}
```

```csharp
// OrderService yalnızca IPricableOrder üzerinden çalışır:
public void PrintTotal(List<IPricableOrder> orders)
{
    foreach (var item in orders)
        Console.WriteLine($"Toplam: {item.GetTotal()}");
}
```

---

### 11. Order Modeli & OrderService
- `Order` modeli: `ItemPrices` listesi üzerinden toplam tutar hesaplar
- `GiftOrder`: fiyatlandırma dışında tutulan hediye siparişi modeli
- `OrderService`: `IPricableOrder` listesi üzerinden tüm sipariş toplamlarını yazdırır
- `ProductsController`'a `OrderService` DI ile enjekte edildi; `GET /api/products/GetTotal` endpoint'i eklendi

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

### 12. Interface Segregation Principle (ISP)
- **Bir sınıf, kullanmadığı metotları içeren arayüzleri implement etmek zorunda kalmamalıdır**
- Büyük `IProductRepository` arayüzü yerine küçük, odaklı arayüzler tanımlandı:

| Arayüz | Sorumluluk |
|---|---|
| `IProductReader` | `GetProduct`, `GetProducts` — sadece okuma |
| `IProductWriter` | `Add`, `Update` — sadece yazma |
| `IProductImporter` | `ImportFromExcel` — dışarıdan içe aktarma |
| `IProductExporter` | `ExportToCsv` — dışarıya aktarma |

- `ProductService` yalnızca okumaya ihtiyaç duyduğu için `IProductReader` aldı, tüm repository arayüzünü değil

```csharp
// ISP uyumlu: Sadece ihtiyaç duyulan arayüzü alır
public ProductService(..., IProductReader productReader, ...)
```

---

### 13. Dependency Inversion Principle (DIP) & Arayüz Tabanlı DI
- **Üst katmanlar, alt katmanlara değil; soyutlamalara bağımlı olmalıdır**
- Tüm servisler artık somut sınıflara değil **arayüzlere** bağımlı
- `ProductService` içinde `new ProductRepository()` / `new ProductPriceCalculator()` gibi doğrudan nesne oluşturma **kaldırıldı**
- Bağımlılıklar constructor injection ile dışarıdan verilir

```csharp
// Önce (DIP ihlali):
_productRepository = new ProductRepository(); // somut sınıfa bağımlı

// Sonra (DIP uyumlu):
public ProductService(IProductReader productReader, IProductPriceCalculator calculator, ...)
```

- `Program.cs`'de arayüz → implementasyon eşleşmeleri DI container'a kaydedildi:

```csharp
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<IProductReader, ProductRepository>();
builder.Services.AddScoped<IProductPriceCalculator, ProductPriceCalculator>();
```

- `ProductsController` da artık `IProductService` ve `IOrderService` arayüzleri üzerinden çalışır
- `ILogger<T>` arayüzü ile yapılandırılmış loglama
- `LogWarning` ile ürün bulunamadığında uyarı loglama
- `LogInformation` ile hesaplanan fiyatı loglama
- `appsettings.json` üzerinden log seviyesi yapılandırması (`Information`, `Warning`)

---

### 14. Entity Framework Core & Veritabanı Katmanı
- **EF Core** ile SQL Server veritabanı entegrasyonu
- `CommerceDbContext` sınıfı ile `DbSet<Product>` ve `DbSet<Category>` tanımı
- `OnModelCreating` içinde **Fluent API** ile ilişki yapılandırması (`HasOne / WithMany / HasForeignKey`)
- `OnDelete(DeleteBehavior.Restrict)` ile kısıtlı silme kuralı
- `HasData` ile **Seed Data** (başlangıç verisi) tanımı
- `EFProductRepository` sınıfı `IProductReader` ve `IProductWriter` arayüzlerini implement eder
- **Eager Loading**: `Include(p => p.Category)` ile ilişkili veriyi tek sorguda çekme
- **Explicit Loading**: `Entry(...).Reference(...).LoadAsync()` ile ilişkiyi ihtiyaç anında yükleme
- Migration tabanlı şema yönetimi: `add-migration`, `update-database`
- `appsettings.json` içindeki `ConnectionStrings:CommerceHubDb` ile bağlantı bilgisi
- `Product` modeline `SKU` (nullable `string?`) alanı eklendi
- `sku_column` migration'u ile `Products` tablosuna `SKU` kolonu eklendi
- `seed_1` migration'u ile ilk ürüne (`Id=1`) seed SKU değeri (`"logi-keyb-1"`) atandı
- `IEntity` arayüzü ile tüm entity'ler için ortak üst tip tanımlandı
- `[Required]`, `[MaxLength]` gibi **Data Annotations** ile model doğrulaması

```csharp
// Eager Loading
return _dbContext.Products.Include(p => p.Category).ToList();
```

```csharp
// Explicit Loading
await _dbContext.Entry(product).Reference(p => p.Category).LoadAsync();
```

```csharp
// DI kaydı
builder.Services.AddDbContext<CommerceDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddScoped<IProductReader, EFProductRepository>();
builder.Services.AddScoped<IProductWriter, EFProductRepository>();
```

---

### 15. Generic Repository Pattern
- **Tekrar eden veri erişim kodunu ortadan kaldırmak** için generic repository tasarım deseni uygulandı
- `IReadRepository<T>` ve `IWriteRepository<T>` arayüzleri ile okuma/yazma operasyonları soyutlandı
- `GenericRepository<T>` sınıfı, `IEntity` kısıtıyla tüm entity türleri için ortak CRUD operasyonlarını implement eder
- `EFCategoryRepository` ve `EFProductRepository`, `GenericRepository<T>`'den türeyerek entity'ye özgü davranış ekleyebilir
- `CategoryService` → `EFCategoryRepository` → `GenericRepository<Category>` zinciri
- `CategoriesController` ile `GET /api/categories` endpoint'i eklendi
- Tüm repository operasyonları **async/await** ile asenkron çalışır

```csharp
// Generic arayüzler
public interface IReadRepository<T> where T : class, IEntity
{
    Task<T> GetByIdAsync(int id);
    Task<IEnumerable<T>> GetAllAsync();
}

public interface IWriteRepository<T> where T : class, IEntity
{
    Task AddAsync(T entity);
    void Update(T entity);
    void Delete(int id);
    Task<int> SaveChangesAsync();
}
```

```csharp
// Generic implementasyon — tüm entity'ler için tek sınıf
public class GenericRepository<T> : IReadRepository<T>, IWriteRepository<T>
    where T : class, IEntity
{
    public async Task<IEnumerable<T>> GetAllAsync()
        => await _dbSet.AsNoTracking().ToListAsync();
}
```

```csharp
// Türetilmiş repository — sadece Category'ye özgü ihtiyaçlar eklenir
public class EFCategoryRepository : GenericRepository<Category> { ... }
```

---

### 16. Ham SQL Sorguları & Raporlama
- `EFProductReportRepository` — koleksiyon üzerinde LINQ ile gruplama yapan rapor repository'si
- `CategorySummary` DTO — `CategoryName`, `ProductsCount`, `AveragePrice` alanlarını taşıyan sonuç modeli
- `ReportsController` — üç farklı yaklaşımın performansını kıyaslayan raporlama controller'ı

**Yaklaşım karşılaştırması:**

| Yaklaşım | Açıklama | Avantaj |
|---|---|---|
| `EFProductReportRepository` | Tüm veriyi belleğe çeker, LINQ ile gruplar | Basit, test edilebilir |
| LINQ + `GroupBy` + `Include` | EF Core'un SQL'e çevirdiği saf LINQ sorgusu | Tek sorgu, verimli |
| `Database.SqlQuery<T>()` | Ham SQL çalıştırır, DTO'ya map eder | Tam kontrol, karmaşık sorgular |

**`FromSqlRaw` yerine `Database.SqlQuery<T>()` kullanılmasının sebebi:**
- `FromSqlRaw`, `DbSet<T>` üzerinde çalışır ve entity'nin tüm zorunlu kolonlarını (özellikle `Id`) SQL sonucunda bekler
- `Database.SqlQuery<T>()` herhangi bir DTO'ya map eder, primary key gerektirmez

```csharp
// ❌ Hatalı: Product entity'si Id kolonunu sonuçta bekler
commerceDbContext.Products.FromSqlRaw("SELECT c.Name, COUNT(*) ...");

// ✅ Doğru: DTO'ya doğrudan map eder
await commerceDbContext.Database
    .SqlQuery<CategorySummary>($@"
        SELECT c.Name AS CategoryName,
               COUNT(*) AS ProductsCount,
               AVG(p.BasePrice) AS AveragePrice
        FROM Products p
        LEFT JOIN Categories c ON p.CategoryId = c.Id
        GROUP BY c.Name")
    .ToListAsync();
```

> **Not:** SQL'deki kolon alias'ları (`CategoryName`, `ProductsCount`, `AveragePrice`), DTO property adlarıyla birebir eşleşmelidir.

---

### 17. FluentValidation
- **FluentValidation** kütüphanesi ile model doğrulaması ASP.NET'in yerleşik `ModelState` mekanizmasından ayrıştırıldı
- `AbstractValidator<T>` sınıfından türeyerek `CreateProductValidator` oluşturuldu
- Kural tanımları constructor içinde `RuleFor(...)` zinciriyle yazılır
- `MustAsync` ile **asenkron, veritabanı destekli** kural: kategori ID varlığı kontrolü
- `CustomAsync` ile **özel hata mesajlı** kural: SKU tekil olma kontrolü
- `IValidator<Product>` arayüzü DI container'a kaydedildi; controller'a constructor injection ile değil, **action injection** (`IValidator<Product> validator` parametresi) ile verildi
- Doğrulama hatalıysa `400 Bad Request` + hata mesajları dönülür

```csharp
// Kural tanımı
RuleFor(p => p.Name)
    .NotEmpty().WithMessage("Ürün adı boş olamaz")
    .MaximumLength(200).WithMessage("Ürün adı en fazla 200 karakter olmalı");

// Async DB kuralı — kategori varlığı kontrolü
RuleFor(p => p.CategoryId)
    .MustAsync(async (categoryId, ct) =>
    {
        var categories = await categoryRepository.GetAllAsync();
        return categories.Any(c => c.Id == categoryId);
    })
    .WithMessage("Belirtilen kategori, kayıtlı değil!");

// Özel async kural — SKU tekil olma
RuleFor(p => p.SKU)
    .CustomAsync(async (sku, context, ct) =>
    {
        var duplicate = (await productReader.GetProductsAsync())
            .FirstOrDefault(p => p.SKU == sku && p.Id != context.InstanceToValidate.Id);
        if (duplicate is not null)
            context.AddFailure(nameof(Product.SKU), $"'{sku}' SKU zaten kullanılıyor");
    });
```

```csharp
// DI kaydı
builder.Services.AddScoped<IValidator<Product>, CreateProductValidator>();

// Controller'da action injection ile kullanım
[HttpPost]
public async Task<IActionResult> CreateNewProduct(Product product, IValidator<Product> validator)
{
    var result = await validator.ValidateAsync(product);
    if (!result.IsValid)
        return BadRequest(result.Errors.Select(e => e.ErrorMessage));
    ...
}
```

---

### 18. Action Filters & Global Validation Filter
- **Action Filter** (`IAsyncActionFilter`) ile controller'lardaki tekrar eden doğrulama kodu merkezileştirildi
- `ValidationFilter` sınıfı her action çalışmadan önce devreye girer; action parametrelerini tarar
- `typeof(IValidator<>).MakeGenericType(...)` ile runtime'da ilgili validator DI container'dan çözülür
- Doğrulama başarısızsa `400 Bad Request` + hata mesajları döner, action hiç çalışmaz
- `AddControllers(option => option.Filters.Add<ValidationFilter>())` ile **global** olarak tüm controller'lara uygulandı
- `CreateNewProduct` action'ındaki inline `validator.ValidateAsync(...)` kodu kaldırıldı; controller temizlendi

```csharp
// ValidationFilter — global, tüm action'lara otomatik uygulanır
public class ValidationFilter : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        foreach (var argument in context.ActionArguments.Values)
        {
            var validatorType = typeof(IValidator<>).MakeGenericType(argument!.GetType());
            var validator = context.HttpContext.RequestServices.GetService(validatorType) as IValidator;

            if (validator is not null)
            {
                var result = await validator.ValidateAsync(new ValidationContext<object>(argument));
                if (!result.IsValid)
                {
                    context.Result = new BadRequestObjectResult(result.Errors.Select(e => e.ErrorMessage));
                    return;
                }
            }
        }
        await next();
    }
}
```

```csharp
// Program.cs — global filter kaydı
builder.Services.AddControllers(option =>
{
    option.Filters.Add<ValidationFilter>();
});
```

---

### 19. DTO Katmanı & Mapster ile Object Mapping
- **DTO (Data Transfer Object)** kullanımıyla entity'ler API katmanına doğrudan expose edilmekten kurtarıldı
- `GetAllProductResponse` — ürün listeleme yanıtı; `CategoryName`, `IsLowStock` gibi hesaplanmış alanlar içerir
- `CreateProductRequest` — ürün oluşturma isteği için `record` tipi DTO
- `CreateProductResponse` — oluşturulan ürünün `Id`'sini döner
- **Mapster** kütüphanesi ile DTO ↔ entity dönüşümü: `request.Adapt<Product>()`
- `TypeAdapterConfig<Product, GetAllProductResponse>.NewConfig()` ile özel mapping kuralı tanımlandı:
  - `IsLowStock = StockCount < 10` hesaplaması mapping sırasında otomatik yapılır
- `IProductService.GetProducts()` artık `IEnumerable<GetAllProductResponse>` döndürür

```csharp
// DTO tanımı (record)
public record CreateProductRequest(string Name, decimal BasePrice, int? CategoryId, string? SKU, ...);
public record CreateProductResponse(int CreatedProductId);

// Mapster ile dönüşüm
var product = request.Adapt<Product>();

// Özel mapping kuralı
TypeAdapterConfig<Product, GetAllProductResponse>.NewConfig()
    .Map(dest => dest.IsLowStock, src => src.StockCount < 10);
```

---

### 20. CQRS Benzeri Command Handler Deseni
- Her özellik (feature) için ayrı bir `Handler` sınıfı oluşturuldu; `IProductService` içindeki fonksiyon sayısının büyümesi önlendi
- `CreateProductCommandHandler` — ürün oluşturma iş mantığını kapsar: request → Mapster → `IProductWriter.AddAsync`
- `Features/` klasör yapısıyla feature-based organizasyon benimsendi:
  - `Features/Products/Commands/CreateNewProduct/` — komut, handler ve yanıt aynı klasörde
  - `Features/DataTransferObjects/` — paylaşılan yanıt DTO'ları
- Controller artık `CreateProductCommandHandler`'ı DI ile alır; `[HttpPost]` action'ı sadece handler'ı çağırır

```csharp
// Handler
public class CreateProductCommandHandler
{
    public async Task<CreateProductResponse> HandleAsync(CreateProductRequest request)
    {
        var product = request.Adapt<Product>();
        await writer.AddAsync(product);
        return new CreateProductResponse(product.Id);
    }
}

// Controller
[HttpPost]
public async Task<IActionResult> CreateNewProduct(CreateProductRequest request)
{
    var response = await _handler.HandleAsync(request);
    return CreatedAtAction(nameof(GetById), new { id = response.CreatedProductId }, response);
}
```

```csharp
// DI kaydı
builder.Services.AddScoped<CreateProductCommandHandler>();
```

---

### 21. Swagger / OpenAPI & Scalar
- `AddSwaggerGen()` ve `AddOpenApi()` ile API dokümantasyonu oluşturuldu
- `UseSwagger()` + `UseSwaggerUI()` ile geliştirme ortamında Swagger UI aktif
- `MapOpenApi()` + `MapScalarApiReference()` ile **Scalar** API istemcisi eklendi
- Scalar, Swagger UI'a alternatif modern bir API keşfetme arayüzü sunar
- Sadece `Development` ortamında aktif (`app.Environment.IsDevelopment()`)

```csharp
// Kayıt
builder.Services.AddSwaggerGen();
builder.Services.AddOpenApi();

// Pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapOpenApi();
    app.MapScalarApiReference();
}
```

| Adres | Açıklama |
|---|---|
| `/swagger` | Swagger UI |
| `/scalar/v1` | Scalar API istemcisi |
| `/openapi/v1.json` | Ham OpenAPI JSON |

---

## 🗂 Proje Yapısı

```
CommerceHub.Web/
├── Controllers/
│   ├── ProductsController.cs          # Ürün API controller
│   ├── CategoriesController.cs        # Kategori API controller
│   └── ReportsController.cs           # Raporlama API controller (3 farklı sorgulama yaklaşımı)
├── Exceptions/
│   └── NotFoundException.cs           # Özel exception sınıfı
├── Middleware/
│   ├── ExceptionHandlingMiddleware.cs  # Merkezi hata yönetimi
│   └── RequestTiminingMiddleware.cs    # İstek süre ölçümü
├── Models/
│   ├── Product.cs                     # Ürün modeli
│   ├── Category.cs                    # Kategori modeli
│   ├── Order.cs                       # Sipariş modeli + IPricableOrder arayüzü + GiftOrder
│   └── CategorySummary.cs             # Raporlama DTO'su (CategoryName, ProductsCount, AveragePrice)
├── Data/
│   └── CommerceDbContext.cs            # EF Core DbContext (Products, Categories)
├── Migrations/                         # EF Core migration dosyaları
├── Repositories/
│   ├── IReadRepository.cs              # Generic okuma arayüzü (IReadRepository<T>, IWriteRepository<T>)
│   ├── GenericRepository.cs           # Generic EF Core repository implementasyonu
│   ├── EFProductRepository.cs         # Ürün repository (GenericRepository<Product>)
│   ├── EFCategoryRepository.cs        # Kategori repository (GenericRepository<Category>)
│   ├── EFProductReportRepository.cs   # Raporlama repository (bellek üzerinde LINQ gruplama)
│   ├── IProductRepository.cs          # ISP uyumlu ürün arayüzleri
│   └── ProductRepository.cs           # In-memory ürün repository (eski)
├── Services/
│   ├── IProductService.cs             # Ürün servis arayüzü
│   ├── IOrderService.cs               # Sipariş servis arayüzü
│   ├── INotificationService.cs        # Bildirim servis arayüzü
│   ├── ICategoryService.cs            # Kategori servis arayüzü
│   ├── IProductPriceCalculator.cs     # Fiyat hesaplama arayüzü
│   ├── ProductService.cs              # İş mantığı koordinasyonu (IProductService impl.)
│   ├── CategoryService.cs             # Kategori iş mantığı (ICategoryService impl.)
│   ├── ProductPriceCalculator.cs      # Fiyat hesaplama (IProductPriceCalculator impl.)
│   ├── OrderService.cs                # Sipariş toplam hesaplama (IOrderService impl.)
│   ├── NotificationService.cs         # Bildirim koordinasyonu (INotificationService impl.)
│   ├── INotification.cs               # Bildirim kanal arayüzü
│   ├── EmailNotification.cs           # E-posta bildirimi
│   ├── SMSNotification.cs             # SMS bildirimi
│   ├── WhatsAppNotification.cs        # WhatsApp bildirimi
│   └── EmailSender.cs                 # (Eski) e-posta gönderici
├── Features/
│   ├── DataTransferObjects/
│   │   └── GetAllProductResponse.cs   # Ürün listeleme yanıt DTO'su (IsLowStock hesaplamalı)
│   └── Products/
│       └── Commands/
│           └── CreateNewProduct/
│               ├── CreateProductRequest.cs        # İstek record DTO'su + CreateProductResponse
│               └── CreateProductCommandHandler.cs # Mapster ile Product oluşturma handler'ı
├── Validators/
│   └── CreateProductValidator.cs      # FluentValidation kuralı (Name, BasePrice, CategoryId, SKU)
├── Filters/
│   └── ValidationFilter.cs            # Global action filter — FluentValidation entegrasyonu
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
| `GET /api/products/GetTotal` | Sipariş toplamlarını hesaplar ve yazdırır        |
| `GET /api/products/Demo/{id}` | Explicit Loading demo endpoint'i               |
| `GET /api/categories`         | Tüm kategorileri listeler (async)               |
| `GET /api/reports/category-summary` | Bellek üzerinde LINQ gruplama ile özet |
| `GET /api/reports/category-summary-alternatif` | EF Core LINQ `GroupBy` sorgusu |
| `GET /api/reports/category-summary-sql` | Ham SQL ile `Database.SqlQuery<T>()` |

---

## 🛠 Kullanılan Teknolojiler

- **.NET 10**
- **ASP.NET Core Minimal API & MVC**
- **Kestrel**
- **Entity Framework Core** (SQL Server)
- **FluentValidation**
- **Mapster** (object mapping)
- **Swagger / OpenAPI** (`Swashbuckle`)
- **Scalar** (modern API istemcisi)
- **Microsoft.Extensions.Options**
- **Microsoft.Extensions.Logging**
- **Microsoft.Extensions.DependencyInjection**
