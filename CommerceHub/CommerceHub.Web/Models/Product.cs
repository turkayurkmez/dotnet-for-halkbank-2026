namespace CommerceHub.Web.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        public decimal BasePrice { get; set; }
        public bool IsOnSale { get; set; }
        public double DiscountRate { get; set; }
        public int StockCount { get; set; }

        public int? CategoryId { get; set; }

        public virtual Category Category { get; set; }

    }
}
