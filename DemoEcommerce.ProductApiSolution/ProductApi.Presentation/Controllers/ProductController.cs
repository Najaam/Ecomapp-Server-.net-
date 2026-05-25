using Microsoft.AspNetCore.Mvc;
using ProductApi.Application.DTOs;
using ProductApi.Application.DTOs.Conversions;
using ProductApi.Application.Interfaces;

namespace ProductApi.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController(IProduct productInterface) : Controller
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetProducts()
        {
            //get all products from repo
            var products = await productInterface.GetAllAsync();
            if (!products.Any())
            {
                return NotFound("No products found in Database.");
            }
            //convert data from entity to dto and return list of products
            var (_, list) = ProductConversions.FromEntity(null, products);
            return list!.Any() ? Ok(list) : NotFound("No Product Found");
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ProductDTO>> GetProductById(int id)
        {
            // get single product from the repo by id
            var product = await productInterface.FindByIdAsync(id);
            if (product == null)
                return NotFound("Requested Product Not Found.");
            //convert data from entity to dto and return list of products
            var (_product, _) = ProductConversions.FromEntity(product, null);
            return _product is not null ? Ok(_product) : NotFound("Requested Product Not Found.");
        }

        [HttpPost]

        public async Task<ActionResult<ProductDTO>> CreateProduct(ProductDTO productDto)
        {
            //check model state is all data annitation are passed 
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var productEntity = ProductConversions.ToEntity(productDto);
            var response = await productInterface.CreateAsync(productEntity);
            return response.Flag is true ? Ok(productDto) : BadRequest(response.Message);
        }
        [HttpPut]
        public async Task<ActionResult<ProductDTO>> UpdateProduct(ProductDTO productDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var productEntity = ProductConversions.ToEntity(productDto);
            var response = await productInterface.UpdateAsync(productEntity);
            return response.Flag is true ? Ok(response) : BadRequest(response.Message);
        }
        [HttpDelete]
        public async Task<ActionResult> DeleteProduct(ProductDTO product)
        {
            var productEntity = ProductConversions.ToEntity(product);
            var response = await productInterface.DeleteAsync(productEntity);
            return response.Flag is true ? Ok(response) : BadRequest(response.Message);
        }
        }
}
