using CommerceHub.Web.Data;
using CommerceHub.Web.Models;
using CommerceHub.Web.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CommerceHub.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {

        private readonly IProductService _productService;
        private readonly IOrderService _orderService;


        public ProductsController(IProductService productService, IOrderService orderService)
        {
            _productService = productService;
            _orderService = orderService;


        }
        [HttpGet]
        public IActionResult GetProducts()
        {

            //ProductService productService = new ProductService();
            var products = _productService.GetProducts();//GetProducts()'ın nasıl çalıştığını BİLMİYOR!
            products.ForEach(p => p.BasePrice = _productService.GetFinalPrice(p.Id));
            //_productService.SendMailToSupplier();
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
            var orders = new List<IPricableOrder>()
            {
                new Order{ ItemPrices = new(){1300,2500}},
               // new GiftOrder{ ItemPrices = new(){ 3000,5000 }, Note="Doğum...." }
            };

            _orderService.PrintTotal(orders);

            return Ok(new { message = "Sonuç konsol'da" });
        }

        [HttpGet("Demo/{id}")]
        public IActionResult ExplicitLoadingDemo(int id, CommerceDbContext commerceDbContext)
        {
            var product = commerceDbContext.Products.Find(id);
            if (product is null)
            {
                return NotFound();
            }

            commerceDbContext.Entry(product).Reference(p => p.Category).Load();
            return Ok(product);
        }

        [HttpGet("/Get/{id:int}")]
        public IActionResult GetById(int id)
        {
            var product = _productService.GetProduct(id);
            return Ok(product);
        }


        [HttpPost]
        public IActionResult CreateNewProduct(Product product)
        {
            _productService.Create(product);
            return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);

        }



    }
}
