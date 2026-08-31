using CommerceHub.Web.Features.DataTransferObjects;
using CommerceHub.Web.Features.Products.Commands.CreateNewProduct;
using MediatR;

namespace CommerceHub.Web.Notifications
{
    public class ProductCreatedNotification : INotification
    {
        public CreateProductResponse CreatedProduct { get; set; }

        public ProductCreatedNotification(CreateProductResponse createdProduct)
        {
            CreatedProduct = createdProduct;
        }
    }

    public class ProductCreatedNotificationHandler : INotificationHandler<ProductCreatedNotification>
    {

        private ILogger<ProductCreatedNotificationHandler> _logger;

        //private INotificationPublisher _publisher;

        public ProductCreatedNotificationHandler(ILogger<ProductCreatedNotificationHandler> logger, INotificationPublisher publisher)
        {
            _logger = logger;
          //  _publisher = publisher;
        }

        public Task Handle(ProductCreatedNotification notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation($" {notification.CreatedProduct.CreatedProductId} id'li ürün başarıyla eklendi");
            //_publisher.Publish()
            return Task.CompletedTask;

        }
    }
}
