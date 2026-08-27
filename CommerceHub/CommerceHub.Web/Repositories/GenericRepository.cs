using CommerceHub.Web.Data;
using CommerceHub.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace CommerceHub.Web.Repositories
{
    public class GenericRepository<T> : IReadRepository<T>, IWriteRepository<T> where T : class, IEntity
    {

        protected readonly CommerceDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public GenericRepository(CommerceDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task AddAsync(T entity) => await _dbSet.AddAsync(entity);
       

        public void Delete(int id)
        {
            var existingEntity = _dbSet.Find(id);
            if (existingEntity is not null)
            {
                _dbSet.Remove(existingEntity);
            }
        }

        public async Task<IEnumerable<T>> GetAllAsync() => await _dbSet.AsNoTracking().ToListAsync();


        public async Task<T> GetByIdAsync(int id) => await _dbSet.FindAsync(id);


        public async Task<int> SaveChangesAsync() => await _context.SaveChangesAsync();
      

        public void Update(T entity) => _dbSet.Update(entity);
     
    }
}
