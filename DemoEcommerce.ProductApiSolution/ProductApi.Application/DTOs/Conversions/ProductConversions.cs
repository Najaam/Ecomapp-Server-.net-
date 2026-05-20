using ProductApi.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductApi.Application.DTOs.Conversions
{
    public static class ProductConversions
    {
        public static Product ToEntity(ProductDTO productDTO) {
            {
                return new Product
                {
                    Id = productDTO.Id,
                    Name = productDTO.Name,
                    Quantity = productDTO.quantity,
                    Price = productDTO.price
                };
            }
        }
       public static (ProductDTO?, IEnumerable<ProductDTO>?)FromEntity(Product? product,   IEnumerable<Product>? products)
        {
            // Return single product
            if (product is not null && products is null)
            {
                var singleProduct = new ProductDTO
                (
                    product.Id,
                    product.Name!,
                    product.Quantity,
                    product.Price
                );

                return (singleProduct, null);
            }

            // Return product list
            if (products is not null && product is null)
            {
                var _products = products.Select(p =>
                    new ProductDTO
                    (
                        p.Id,
                        p.Name!,
                        p.Quantity ?? 0,
                        p.Price ?? 0
                    )
                ).ToList();

                return (null, _products);
            }

            // Invalid case
            return (null, null);
        }
    }
}
