namespace CommerceHub.Web.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal BasePrice { get; set; }
        public bool IsOnSale { get; set; }
        public double DiscountRate { get; set; }
    }
}
