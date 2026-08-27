using CommerceHub.Web.Models;
using CommerceHub.Web.Repositories;

namespace CommerceHub.Web.Services
{
    public interface ICategoryService
    {
        Task<IEnumerable<Category>> GetCategoriesAsync();
    }
    public class CategoryService : ICategoryService
    {

        private readonly EFCategoryRepository categoryRepository;

        public CategoryService(EFCategoryRepository categoryRepository)
        {
            this.categoryRepository = categoryRepository;
        }

        public async Task<IEnumerable<Category>> GetCategoriesAsync()
        {
           return await categoryRepository.GetAllAsync();
        }
    }
}
