using MediatR;

namespace CommerceHub.Web.Features.Products.Commands.CreateNewProduct
{
    public record CreateProductRequest  (
        string Name, 
        string? Description, 
        decimal BasePrice,
        bool IsOnSale, 
        double DiscountRate, 
        int StockCount, 
        int? CategoryId, 
        string? SKU
    ) : IRequest<CreateProductResponse>;
    //1. MediatR paketi aracılığıyla Mediator patternini inşa ediyoruz. İlk adım isteği (handler parametresini)IRequest ile imzaladık.


    public record CreateProductResponse(int CreatedProductId);
}
