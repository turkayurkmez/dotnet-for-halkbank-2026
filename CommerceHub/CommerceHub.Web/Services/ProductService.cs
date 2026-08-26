using CommerceHub.Web.Models;

namespace CommerceHub.Web.Services
{
    public class ProductService : IProductService
    {
        //private readonly List<Product> _products = new()
        //{
        //    new Product {Id=1, Name = "Bluetooth kulaklık", BasePrice=1200m, IsOnSale=true, DiscountRate=0.15 },
        //    new Product {Id=2, Name = "Logitech Klavye", BasePrice=2500m, IsOnSale=false, DiscountRate=0 }

        //};

        private IProductReader _productReader; //Bu nesneler olmadığında, bu sınıf çalışmaz. Demek ki bunların hepsi dependency
        private IProductPriceCalculator _calculator;
        private INotificationService _notification;
        private ILogger<ProductService> _logger;
        private readonly IProductWriter _productWriter;

        public ProductService(ILogger<ProductService> logger, INotificationService notificationService, IProductReader productReader, IProductPriceCalculator calculator, IProductWriter productWriter)
        {

            //eğer bağımlı olduğunuc bir nesnenin instance'ını sınıfın içinde alıyorsanız, prensibi ihlal ediyorsunuz...
            //_productRepository = new ProductRepository();
            //_calculator = new ProductPriceCalculator();

            _productReader = productReader;
            _notification = notificationService;
            _calculator = calculator;
            _logger = logger;
            _productWriter = productWriter;
        }

        public decimal GetFinalPrice(int id)
        {
            //önce ürünü bul. Eğer indirimdeyse, indirim oranını base fiyata uygula
            //var product = _products.FirstOrDefault(p => p.Id == id);
            var product = _productReader.GetProduct(id);
            if (product is null)
            {
                // Console.WriteLine($"[ProductService] id'si {id} olan ürün bulunamadı!");
                _logger.LogWarning($"id'si {id} olan ürün bulunamadı!");
                throw new KeyNotFoundException($"id'si {id} olan ürün bulunamadı!");
            }

            //decimal finalPrice = product.BasePrice;
            //if (product.IsOnSale)
            //{
            //    finalPrice *= 1 - (decimal)product.DiscountRate;
            //}

            decimal finalPrice = _calculator.CalculateFinalPrice(product);

            // Console.WriteLine($"[ProductService] {product.Name} için hesaplanan indirimli fiyat: {finalPrice}");
            _logger.LogInformation($"{product.Name} için hesaplanan indirimli fiyat: {finalPrice}");
            return finalPrice;
        }
        public List<Product> GetProducts() => _productReader.GetProducts().ToList();

        public void SendMailToSupplier()
        {
            EmailNotification emailNotification = new EmailNotification();
            WhatsAppNotification whatsAppNotification = new WhatsAppNotification();
            SMSNotification sMSNotification = new SMSNotification();
            _notification.Notify(sMSNotification, "Test mesajı");
            _logger.LogInformation("Gönderim yapıldı");
        }

        public void CreateProduct(Product product) {
            _productWriter.Add(product);
        }

        public void Create(Product product)
        {
            _productWriter.Add(product);
        }

        public void Update(Product product)
        {
            _productWriter.Update(product);
        }

        public void Delete(int id)
        {
            _productWriter.Delete(id);
        }

        public Product GetProduct(int id)
        {
            return _productReader.GetProduct(id);
        }




        /*
         *  Bu sınıfta değişiklik yapmamı gerektirecek kaç durum var?
         *  ProductService'in dili olsa ve ona sorumluluklarını sorsak bize ne yanıt verir?
         *  
         *  1. DB ile ilgili işlemleri yapıyor (DB adresi, erişim tekniği, tablo yapısı vs değişebilir)
         *  2. Mail atmak (eposta servisi değişebilir, template'ler değişebilir)
         *  
         *  GetFinalPrice'a özel soruyoruz:
         *  DB Değişikliği
         *  İndirim kuralı değişirse
         *  Loglama stratejisi değişirse.
         *  
         */
    }
}
