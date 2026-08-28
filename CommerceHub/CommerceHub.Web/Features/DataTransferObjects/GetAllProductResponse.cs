namespace CommerceHub.Web.Features.DataTransferObjects
{
    public class GetAllProductResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal BasePrice { get; set; }
        public int StockCount { get; set; }
        public string? CategoryName { get; set; }

        public string SKU { get; set; } = string.Empty;
        public bool IsLowStock { get; set; }
    }
}
