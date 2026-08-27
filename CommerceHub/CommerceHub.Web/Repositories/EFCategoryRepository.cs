using CommerceHub.Web.Data;
using CommerceHub.Web.Models;

namespace CommerceHub.Web.Repositories
{
    public class EFCategoryRepository : GenericRepository<Category>
    {
        public EFCategoryRepository(CommerceDbContext context) : base(context)
        {
        }


    }
}
