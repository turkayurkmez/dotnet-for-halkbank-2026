namespace CommerceHub.Web.Models
{
    public class CategorySummary
    {
        public string CategoryName { get; set; } = string.Empty;
        public int ProductsCount { get; set; }
        public decimal AveragePrice { get; set; }

    }
}
