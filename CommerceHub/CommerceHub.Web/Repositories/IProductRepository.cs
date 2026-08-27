using CommerceHub.Web.Models;

namespace CommerceHub.Web.Repositories
{
    public interface IProductRepository
    {   

    }

    public interface IProductExporter
    {
        void ExportToCsv(string filePath);
    }


    public interface IProductImporter
    {
        void ImportFromExcel(string filePath);
    }

    public interface IProductReader
    {
      
        Task<Product?> GetProductAsync(int id);
       
        Task<IEnumerable<Product>> GetProductsAsync();
    }

    public interface IProductWriter
    {      
        Task DeleteAsync(int id);
        Task AddAsync(Product product);
        Task UpdateAsync(Product product);
    }



    //public class ProductRepositoryForReport : IProductReader
    //{
    //    public Product? GetProduct(int id)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public IEnumerable<Product> GetProducts()
    //    {
    //        throw new NotImplementedException();
    //    }
    //}

    //ISP prensibi: Bir sınıf, bir interface'i implemente ediyorsa; implente ettiği tüm fonksiyonları KULLANMAK ZORUNDA
}
