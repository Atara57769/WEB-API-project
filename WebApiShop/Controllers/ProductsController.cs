using DTOs;
using Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace WebApiShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }


        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<PageResponseDTO<ProductDTO>>> Get([FromQuery] int?[] categoryIds, string? description, int? maxPrice, int? minPrice, int position = 1, int skip = 8)
        {
            var pageResponse = await _productService.GetProducts(
                position, skip, categoryIds, description, maxPrice, minPrice);

            if (pageResponse == null || !pageResponse.Data.Any()) return NoContent();

            return Ok(pageResponse);
        }


        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<ProductDTO>> Get(int id)
        {
            var product = await _productService.GetProductById(id);
            if (product == null) return NotFound();

            return Ok(product);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<ProductDTO>> Post([FromBody] PostProductDTO newProduct)
        {
            var returnedProduct = await _productService.AddProduct(newProduct);
            if (returnedProduct == null) return BadRequest();

            return CreatedAtAction(nameof(Get), new { id = returnedProduct.Id }, returnedProduct);
        }



    }
}