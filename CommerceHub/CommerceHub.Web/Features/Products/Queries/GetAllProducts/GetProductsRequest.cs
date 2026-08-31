using CommerceHub.Web.Features.DataTransferObjects;
using MediatR;

namespace CommerceHub.Web.Features.Products.Queries.GetAllProducts
{
    public class GetProductsRequest : IRequest<IEnumerable<GetAllProductResponse>>
    {
    }


}
