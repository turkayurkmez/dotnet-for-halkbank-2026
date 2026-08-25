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

        public ProductsController(ProductService productService)
        {
            _productService = productService;
        }
        [HttpGet]
        public IActionResult GetProducts()
        {
            
            //ProductService productService = new ProductService();
            var products = _productService.GetProducts();
            products.ForEach(p => p.BasePrice = _productService.GetFinalPrice(p.Id));
            return Ok(products);
        }

        [HttpGet("{id:int}")]
        public IActionResult GetFinalPriceOf(int id)
        {
            //ProductService productService = new ProductService();
            var price = _productService.GetFinalPrice(id);
            return Ok(price);
        }
    }
}
