using CommerceHub.Web.Data;
using CommerceHub.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace CommerceHub.Web.Repositories
{
    public class EFProductRepository : GenericRepository<Product>, IProductReader, IProductWriter
    {



        public EFProductRepository(CommerceDbContext context) : base(context)
        {
        }

        public async Task AddAsync(Product product)
        {
            await base.AddAsync(product);
            await base.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            base.Delete(id);
            await base.SaveChangesAsync();
        }

        public async Task<Product?> GetProductAsync(int id)
        {
            return await base.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Product>> GetProductsAsync()
        {
            //Eager Loading:
            return await base.GetAllAsync();
        }

        public async Task UpdateAsync(Product product)
        {

            base.Update(product);
            await base.SaveChangesAsync();
        }
    }
}
