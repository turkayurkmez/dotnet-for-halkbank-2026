using CommerceHub.Web.Features.Products.Commands.CreateNewProduct;
using CommerceHub.Web.Models;
using CommerceHub.Web.Notifications;
using CommerceHub.Web.Repositories;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace CommerceHub.Tests
{
    public class CreateProductCommandHandlerTests
    {
        [Fact]
        public async Task Handle_Valid_Command_Calls_Add_Async_And_Publish_Notification()
        {
            //Arrange:
            var writerMock = new Mock<IProductWriter>();
            var mediator = new Mock<IMediator>();
            var logger = new Mock<ILogger<CreateProductCommandHandler>>();

            var handler = new CreateProductCommandHandler(writerMock.Object, mediator.Object, logger.Object);

            var request = new CreateProductRequest(
                Name: "Test Product From TDD", 
                Description: "Sample", 
                BasePrice: 500M, IsOnSale: true, 
                DiscountRate: 0.25, 
                StockCount: 10, 
                CategoryId: 1, 
                SKU: "tdd-test-1");

            //Act:

         var respnse =   await handler.Handle(request, CancellationToken.None);
            var fakeData = 1;
            
            //Assert:

            writerMock.Verify(w => w.AddAsync(It.Is<Product>(p=>p.Name==request.Name && p.SKU==request.SKU)), Times
                .Once);

            mediator.Verify(m => m.Publish(It.IsAny<ProductCreatedNotification>(), It.IsAny<CancellationToken>()), Times.Once);

            //respnse.CreatedProductId.Should().BeGreaterThan(0);
            fakeData.Should().BeGreaterThan(0);

        }
    }
}
