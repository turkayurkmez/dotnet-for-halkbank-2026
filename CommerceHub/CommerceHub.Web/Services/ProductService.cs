using CommerceHub.Web.Models;

namespace CommerceHub.Web.Services
{
    public class ProductService
    {
        private readonly List<Product> _products = new()
        {
            new Product {Id=1, Name = "Bluetooth kulaklık", BasePrice=1200m, IsOnSale=true, DiscountRate=0.15 },
            new Product {Id=2, Name = "Logitech Klavye", BasePrice=2500m, IsOnSale=false, DiscountRate=0 }

        };

        public decimal GetFinalPrice(int id)
        {
            //önce ürünü bul. Eğer indirimdeyse, indirim oranını base fiyata uygula
            var product = _products.FirstOrDefault(p => p.Id == id);
            if (product is null)
            {
                Console.WriteLine($"[ProductService] id'si {id} olan ürün bulunamadı!");
                throw new KeyNotFoundException($"id'si {id} olan ürün bulunamadı!");
            }

            decimal finalPrice = product.BasePrice;
            if (product.IsOnSale)
            {
                finalPrice *= 1 - (decimal)product.DiscountRate;
            }

            Console.WriteLine($"[ProductService] {product.Name} için hesaplanan indirimli fiyat: {finalPrice}");
            return finalPrice;
        }
    }
}
