using CommerceHub.Web.Models;
using CommerceHub.Web.Repositories;
using Mapster;

namespace CommerceHub.Web.Features.Products.Commands.CreateNewProduct
{
    public class CreateProductCommandHandler
    {
        private readonly IProductWriter writer;

        public CreateProductCommandHandler(IProductWriter writer)
        {
            this.writer = writer;
        }

        public async Task<CreateProductResponse> HandleAsync(CreateProductRequest request)
        {
            var product = request.Adapt<Product>();
            await writer.AddAsync(product);

            return new CreateProductResponse(product.Id);

        }
    }
}
