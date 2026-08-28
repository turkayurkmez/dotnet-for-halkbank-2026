using CommerceHub.Web.Models;
using CommerceHub.Web.Repositories;

namespace CommerceHub.Web.Services
{
    public interface ICategoryService
    {
        Task<IEnumerable<Category>> GetCategoriesAsync();
        Task Create(Category category);
    }
    public class CategoryService : ICategoryService
    {

        private readonly EFCategoryRepository categoryRepository;

        public CategoryService(EFCategoryRepository categoryRepository)
        {
            this.categoryRepository = categoryRepository;
        }

        public async Task Create(Category category)
        {
            await categoryRepository.AddAsync(category);
            await categoryRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<Category>> GetCategoriesAsync()
        {
           return await categoryRepository.GetAllAsync();
        }
    }
}
