namespace CommerceHub.Web.Features.Products.Commands.CreateNewProduct
{
    public record CreateProductRequest(
        string Name, 
        string? Description, 
        decimal BasePrice,
        bool IsOnSale, 
        bool DiscountRate, 
        int StockCount, 
        int? CategoryId, 
        string? SKU
    );


    public record CreateProductResponse(int CreatedProductId);
}
