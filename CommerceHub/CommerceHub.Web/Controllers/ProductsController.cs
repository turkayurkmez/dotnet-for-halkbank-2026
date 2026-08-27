using CommerceHub.Web.Data;
using CommerceHub.Web.Models;
using CommerceHub.Web.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

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
        public async Task<IActionResult> GetProducts()
        {

            //ProductService productService = new ProductService();
            var products = await _productService.GetProducts();//GetProducts()'ın nasıl çalıştığını BİLMİYOR!

            //maaliyeti düşürmek için, indirim hesaplamaktan vazgeçtik.
           //products.ForEach(p => p.BasePrice = _productService.GetFinalPrice(p.Id));
            //_productService.SendMailToSupplier();
            return Ok(products);
        }

        //[HttpGet("{id:int}")]
        //public IActionResult GetFinalPriceOf(int id)
        //{
        //    //ProductService productService = new ProductService();
        //    var price = _productService.GetFinalPrice(id);
        //    return Ok(price);
        //}
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

        //[HttpGet("Demo/{id}")]
        //public IActionResult ExplicitLoadingDemo(int id, CommerceDbContext commerceDbContext)
        //{
        //    var product = commerceDbContext.Products.Find(id);
        //    if (product is null)
        //    {
        //        return NotFound();
        //    }

        //    commerceDbContext.Entry(product).Reference(p => p.Category).Load();
        //    return Ok(product);
        //}

        [HttpGet("/Get/{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var product = await _productService.GetProduct(id);
            return Ok(product);
        }


        [HttpPost]
        public async Task<IActionResult> CreateNewProduct(Product product)
        {
            await _productService.Create(product);
            return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);

        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Product product)
        {
            if (id != product.Id)
            {
                return BadRequest("URL'deki parametre ile güncel verinin id'si eşleşmiyor!!");

            }

            var existing = await _productService.GetProduct(id);
            if (existing is null)
            {
                return NotFound();
            }

           await _productService.Update(product);
            return NoContent();



        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existing = await _productService.GetProduct(id);
            if (existing is null)
            {
                return NotFound();                
            }
            await _productService.Delete(id);
            return NoContent();
        }

        //[HttpPost("coklu-yavas")]
        //public IActionResult AddWithSlow(CommerceDbContext context)
        //{
        //    var stopWatch = Stopwatch.StartNew();
        //    for (int i = 0; i < 1000; i++)
        //    {
        //        context.Products.Add(new Product { Name = $"TestProduct{i}", BasePrice = 10, CategoryId = 1, StockCount = 0 });
        //        context.SaveChanges();
        //    }

        //    stopWatch.Stop();
        //    return Ok(new { message = $"Geçen süre: {stopWatch.ElapsedMilliseconds} ms geçti" });
         

        //}

        //[HttpPost("coklu-hizli")]
        //public IActionResult AddWithFast(CommerceDbContext context)
        //{
        //    var stopWatch = Stopwatch.StartNew();
        //    var products = Enumerable.Range(0, 1000)
        //                   .Select(i => new Product { Name = $"Test Ürün {i}", BasePrice = 10, CategoryId = 1, StockCount = 0 })
        //                   .ToList();

        //    context.Products.AddRange(products);
        //    context.SaveChanges();

        //    stopWatch.Stop();
        //    return Ok(new { message = $"Geçen süre: {stopWatch.ElapsedMilliseconds} ms geçti" });


        //}




    }
}
