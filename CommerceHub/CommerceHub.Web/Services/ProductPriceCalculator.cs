using CommerceHub.Web.Models;

namespace CommerceHub.Web.Services
{
    /// <summary>
    /// Bu sınıf, bir ürünün fiyatını hesaplamaktan sorumludur.
    /// </summary>
    public class ProductPriceCalculator
    {
        public decimal CalculateFinalPrice(Product product)
        {
            if (!product.IsOnSale)
            {
                return product.BasePrice;
            }

            return product.BasePrice * (1 - (decimal)product.DiscountRate);
        }
    }
}
