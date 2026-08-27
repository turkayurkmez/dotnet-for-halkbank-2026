using CommerceHub.Web.Data;
using CommerceHub.Web.Models;
using CommerceHub.Web.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace CommerceHub.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        private readonly EFProductReportRepository _report;

        public ReportsController(EFProductReportRepository report)
        {
            _report = report;
        }

        [HttpGet("category-summary")]
        public async Task<IActionResult> GetCategorySummary()
        {
            var stopWatch = Stopwatch.StartNew(); 
            var summary = await _report.GetCategorySummariesAsync();
            stopWatch.Stop();

            return Ok(new { response = summary, message =$"Önce koleksiyon sonra group by, {stopWatch.ElapsedMilliseconds} ms sürdü. " });

        }

        [HttpGet("category-summary-alternatif")]
        public async Task<IActionResult> GetCategorySummaryAlternate(CommerceDbContext commerceDbContext)
        {
            var stopWatch = Stopwatch.StartNew();

            var summary = await commerceDbContext.Products.Include(p => p.Category)
                                                          .GroupBy(p => p.Category!.Name)
                                                          .Select(g => new CategorySummary
                                                          {
                                                              CategoryName = g.Key,
                                                              ProductsCount = g.Count(),
                                                              AveragePrice = g.Average(p => p.BasePrice)
                                                          })
                                                          .OrderByDescending(r => r.AveragePrice)
                                                          .ToListAsync();

            stopWatch.Stop();


            return Ok(new { response = summary, message = $"Doğrudan group by, {stopWatch.ElapsedMilliseconds} ms sürdü. " });
        }

        [HttpGet("category-summary-sql")]
        public async Task<IActionResult> GetCategorySummaryAlternate2(CommerceDbContext commerceDbContext)
        {
            var stopWatch = Stopwatch.StartNew();

            var summary = await commerceDbContext.Database
                .SqlQuery<CategorySummary>($@"
                    SELECT c.Name AS CategoryName, COUNT(*) AS ProductsCount, AVG(p.BasePrice) AS AveragePrice
                    FROM Products p
                    LEFT JOIN Categories c ON p.CategoryId = c.Id
                    GROUP BY c.Name
                    ORDER BY AveragePrice DESC")
                .ToListAsync();

            stopWatch.Stop();


            return Ok(new { response = summary, message = $"Doğrudan SQL, {stopWatch.ElapsedMilliseconds} ms sürdü." });
        }

    }
}
