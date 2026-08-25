using CommerceHub.Web.Models;
using CommerceHub.Web.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CommerceHub.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {

        private readonly ProductService _productService;
        private readonly OrderService _orderService;

        public ProductsController(ProductService productService, OrderService orderService)
        {
            _productService = productService;
            _orderService = orderService;


        }
        [HttpGet]
        public IActionResult GetProducts()
        {
            
            //ProductService productService = new ProductService();
            var products = _productService.GetProducts();
            products.ForEach(p => p.BasePrice = _productService.GetFinalPrice(p.Id));
            _productService.SendMailToSupplier();
            return Ok(products);
        }

        [HttpGet("{id:int}")]
        public IActionResult GetFinalPriceOf(int id)
        {
            //ProductService productService = new ProductService();
            var price = _productService.GetFinalPrice(id);
            return Ok(price);
        }
        [HttpGet("GetTotal")]
        public IActionResult GetOrderTotal()
        {
            var orders = new List<Order>()
            {
                new Order{ ItemPrices = new(){1300,2500}},
                new GiftOrder{ ItemPrices = new(){ 3000,5000 }, Note="Doğum...." }
            };

            _orderService.PrintTotal(orders);

            return Ok(new { message = "Sonuç konsol'da" });
        }
    }
}
