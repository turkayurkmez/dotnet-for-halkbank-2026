using CommerceHub.Web.Models;

namespace CommerceHub.Web.Services
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
        Product? GetProduct(int id);
        IEnumerable<Product> GetProducts();
    }

    public interface IProductWriter
    {
        void Add(Product product);
        void Update(Product product);
        void Delete(Product product);
    }



    public class ProductRepositoryForReport : IProductReader
    {
        public Product? GetProduct(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Product> GetProducts()
        {
            throw new NotImplementedException();
        }
    }

    //ISP prensibi: Bir sınıf, bir interface'i implemente ediyorsa; implente ettiği tüm fonksiyonları KULLANMAK ZORUNDA
}
