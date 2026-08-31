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

        public CreateProductCommandHandler(IProductWriter writer, IMediator mediator)
        {
            this.writer = writer;
            _mediator = mediator;
        }

        public async Task<CreateProductResponse> Handle(CreateProductRequest request, CancellationToken cancellationToken)
        {
            var product = request.Adapt<Product>();
            await writer.AddAsync(product);

            var response = new CreateProductResponse(product.Id);
            await _mediator.Publish(new ProductCreatedNotification(response));
            return response;
        }


    }
}
