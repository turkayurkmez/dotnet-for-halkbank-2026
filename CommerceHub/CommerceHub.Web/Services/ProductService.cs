using CommerceHub.Web.Models;

namespace CommerceHub.Web.Services
{
    public class ProductService
    {
        //private readonly List<Product> _products = new()
        //{
        //    new Product {Id=1, Name = "Bluetooth kulaklık", BasePrice=1200m, IsOnSale=true, DiscountRate=0.15 },
        //    new Product {Id=2, Name = "Logitech Klavye", BasePrice=2500m, IsOnSale=false, DiscountRate=0 }

        //};

        private ProductRepository _productRepository;
        private ProductPriceCalculator _calculator;
        private EmailSender _sender;
        private ILogger<ProductService> _logger;

        public ProductService(ILogger<ProductService> logger)
        {
            _productRepository = new ProductRepository();
            _calculator = new ProductPriceCalculator();
            _sender = new EmailSender();
            _logger = logger;
        }

        public decimal GetFinalPrice(int id)
        {
            //önce ürünü bul. Eğer indirimdeyse, indirim oranını base fiyata uygula
            //var product = _products.FirstOrDefault(p => p.Id == id);
            var product = _productRepository.GetProduct(id);
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
        public List<Product> GetProducts() => _productRepository.GetProducts();

        public void SendMailToSupplier()
        {
            _sender.SendEmailToSupplier();
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
