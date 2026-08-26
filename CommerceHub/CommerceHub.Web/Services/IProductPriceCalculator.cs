using CommerceHub.Web.Models;

namespace CommerceHub.Web.Services
{
    public interface IProductPriceCalculator
    {
        decimal CalculateFinalPrice(Product product);
    }
}