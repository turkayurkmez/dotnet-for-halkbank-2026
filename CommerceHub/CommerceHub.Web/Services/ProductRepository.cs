using CommerceHub.Web.Models;

namespace CommerceHub.Web.Services
{
    /// <summary>
    /// Bu sınıfın sorumluluğu, Product verisi ile (db) doğrudan çalışmaktır.
    /// </summary>
    public class ProductRepository : IProductReader 
    {
        private readonly List<Product> _products = new()
        {
            new Product {Id=1, Name = "Bluetooth kulaklık", BasePrice=1200m, IsOnSale=true, DiscountRate=0.15 },
            new Product {Id=2, Name = "Logitech Klavye", BasePrice=2500m, IsOnSale=false, DiscountRate=0 }

        };

        public IEnumerable<Product> GetProducts() => _products;

        public Product? GetProduct(int id)
        {
            return _products.FirstOrDefault(p => p.Id == id);
        }
    }
}
