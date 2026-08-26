using CommerceHub.Web.Models;

namespace CommerceHub.Web.Services
{
    public interface IProductService
    {
        decimal GetFinalPrice(int id);
        List<Product> GetProducts();
        void SendMailToSupplier();
    }
}