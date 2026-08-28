using CommerceHub.Web.Models;

namespace CommerceHub.Web.Services
{
    public interface IProductService
    {
        Task<decimal> GetFinalPrice(int id);
        Task<List<Product>> GetProducts();
        Task<Product> GetProduct(int id);
        void SendMailToSupplier();

        Task Create(Product product);
        Task Update(Product product);
        Task Delete(int id);

        Task<IEnumerable<Product>> Search(string keyword);
    }
}