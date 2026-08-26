using CommerceHub.Web.Models;

namespace CommerceHub.Web.Services
{
    public interface IProductService
    {
        decimal GetFinalPrice(int id);
        List<Product> GetProducts();
        Product GetProduct(int id);
        void SendMailToSupplier();

        void Create(Product product);
        void Update(Product product);
        void Delete(int id);
    }
}