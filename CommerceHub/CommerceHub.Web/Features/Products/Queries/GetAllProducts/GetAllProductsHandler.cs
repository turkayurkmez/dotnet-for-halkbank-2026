using CommerceHub.Web.Features.DataTransferObjects;
using CommerceHub.Web.Repositories;
using Mapster;
using MediatR;
using System.Collections;

namespace CommerceHub.Web.Features.Products.Queries.GetAllProducts
{
    public class GetAllProductsHandler : IRequestHandler<GetProductsRequest, IEnumerable<GetAllProductResponse>>
    {

        private readonly IProductReader _reader;

        public GetAllProductsHandler(IProductReader reader)
        {
            _reader = reader;
        }

        public async Task<IEnumerable<GetAllProductResponse>> Handle(GetProductsRequest request, CancellationToken cancellationToken)
        {
            var products = await _reader.GetProductsAsync();
            return products.Adapt<IEnumerable<GetAllProductResponse>>();
        }
    }
}
