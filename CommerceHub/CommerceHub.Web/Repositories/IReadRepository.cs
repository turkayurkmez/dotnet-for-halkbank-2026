using CommerceHub.Web.Models;

namespace CommerceHub.Web.Repositories
{
    public interface IReadRepository<T> where T: class,IEntity
    {
        Task<T> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync(); 
    }

    public interface IWriteRepository<T> where T:class, IEntity
    {
        Task AddAsync(T entity);
        void Update(T entity);
        void Delete(int id);

        Task<int> SaveChangesAsync();
     
    }
}
