using CommerceHub.Web.Models;

namespace CommerceHub.Web.Repositories
{
    public class EFProductReportRepository
    {
        private readonly IProductReader _reader;

        public EFProductReportRepository(IProductReader reader)
        {
            _reader = reader;
        }

        public async Task<List<CategorySummary>> GetCategorySummariesAsync()
        {

          var products = await _reader.GetProductsAsync();


            //DİKKAT!!! Burada, LINQ, SQL'e ÇEVRİLMİYOR!!!!! Doğrudan IEnumerable<Product> üzerinde çalışıyor.
            var summary = products.Where(p => p.Category is not null)
                                  .GroupBy(p => p.Category!.Name)
                                  .Select(r => new CategorySummary
                                  {
                                      CategoryName = r.Key,
                                      ProductsCount = r.Count(),
                                      AveragePrice = r.Average(p => p.BasePrice)
                                  })
                                  .OrderByDescending(s => s.AveragePrice)
                                  .ToList();



            return summary;

        }
    }
}
