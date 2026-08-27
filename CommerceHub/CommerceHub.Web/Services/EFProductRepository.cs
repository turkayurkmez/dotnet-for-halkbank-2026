using CommerceHub.Web.Data;
using CommerceHub.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace CommerceHub.Web.Services
{
    public class EFProductRepository : IProductReader, IProductWriter
    {

        private readonly CommerceDbContext _dbContext;

        public EFProductRepository(CommerceDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Add(Product product)
        {
            _dbContext.Products.Add(product);
            _dbContext.SaveChanges();
        }

        public void Delete(int id)
        {
            var product = _dbContext.Products.Find(id);
            if (product is not null)
            {
                _dbContext.Products.Remove(product);
                _dbContext.SaveChanges();
            }
        }

        public Product? GetProduct(int id)
        {
           return _dbContext.Products.AsNoTracking().FirstOrDefault(x => x.Id == id);                
        }

        public IEnumerable<Product> GetProducts()
        {
            //Eager Loading:
            return _dbContext.Products.Include(p=>p.Category).ToList();
        }

        public void Update(Product product)
        {
            
            _dbContext.Products.Update(product);
            _dbContext.SaveChanges();
        }
    }
}
