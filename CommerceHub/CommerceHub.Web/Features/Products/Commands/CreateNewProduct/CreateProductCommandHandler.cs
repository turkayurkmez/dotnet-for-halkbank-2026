using Azure;
using CommerceHub.Web.Features.DataTransferObjects;
using CommerceHub.Web.Models;
using CommerceHub.Web.Notifications;
using CommerceHub.Web.Repositories;
using Mapster;
using MediatR;

namespace CommerceHub.Web.Features.Products.Commands.CreateNewProduct
{
    public class CreateProductCommandHandler : IRequestHandler<CreateProductRequest, CreateProductResponse>
    {
        private readonly IProductWriter writer;
        private readonly IMediator _mediator;
        private readonly ILogger<CreateProductCommandHandler> _logger;

        public CreateProductCommandHandler(IProductWriter writer, IMediator mediator, ILogger<CreateProductCommandHandler> logger)
        {
            this.writer = writer;
            _mediator = mediator;
            _logger = logger;
        }

        public async Task<CreateProductResponse> Handle(CreateProductRequest request, CancellationToken cancellationToken)
        {
            var product = request.Adapt<Product>();
            await writer.AddAsync(product);

            _logger.LogInformation("Ürün oluşturuldu. ProductId:{id}, Sku:{sku}, categoryID:{catId}", product.Id, product.SKU, product.CategoryId);
            var response = new CreateProductResponse(product.Id);

            await _mediator.Publish(new ProductCreatedNotification(response));
            return response;
        }


    }
}
