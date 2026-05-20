using eCommrece.SharedLibrary.Logs;
using eCommrece.SharedLibrary.Responses;
using ProductApi.Application.Interfaces;
using ProductApi.Domain.Entities;
using ProductApi.Infrastructure.Data;
using System.Linq.Expressions;

namespace ProductApi.Infrastructure.Repositories
{
    internal class ProductRepository(ProductDbContext _context) : IProduct
    { 
        public async Task<Response> CreateAsync(Product entity)
        {
            try
            {
                // if product already exists, return a response with a message indicating that the product already exists
                var getProduct  = await GetByAsync(p=> p.Name! == entity.Name);
                if (getProduct != null && !string.IsNullOrEmpty(getProduct.Name)) {
                    return new Response(false, $"{entity.Name} already exists.");
                }
                var currentEntity = _context.Products.Add(entity).Entity;
                await _context.SaveChangesAsync();
                if(currentEntity != null && currentEntity.Id > 0) {
                    return new Response(true, $"{entity.Name} created successfully.");
                }
                else
                {
                    return new Response(false, $"Failed to create {entity.Name}.");
                }
            }
            catch (Exception ex) {
                // Log the orignal exception using the LogException class
                LogException.LogExceptions(ex);

                return new Response(false, "An error occurred while creating the product.");
            }
        }

        public Task<Response> DeleteAsync(Product entity)
        {
            throw new NotImplementedException();
        }

        public Task<Product> FindByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Product>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Product> GetByAsync(Expression<Func<Product, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Task<Response> UpdateAsync(Product entity)
        {
            throw new NotImplementedException();
        }
    }
}
