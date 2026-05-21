using Microsoft.AspNetCore.Mvc;
using ProductApi.Application.DTOs;
using ProductApi.Application.DTOs.Conversions;
using ProductApi.Application.Interfaces;

namespace ProductApi.Presentation.Controllers
{
    public class ProductController(IProduct productInterface) : Controller
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetProducts()
        {
            var products = await productInterface.GetAllAsync();
            if (products.Any())
            {
                return NotFound("No products found in Database.");
            }
           var(_,list) = ProductConversions.FromEntity(null, products);
            return list!.Any()? Ok(list):NotFound("No Product Found");
        }
        
    }
}
