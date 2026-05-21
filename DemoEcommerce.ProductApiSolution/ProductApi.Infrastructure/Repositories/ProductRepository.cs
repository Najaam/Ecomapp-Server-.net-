using eCommrece.SharedLibrary.Logs;
using eCommrece.SharedLibrary.Responses;
using Microsoft.EntityFrameworkCore;
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
                var getProduct = await GetByAsync(p => p.Name! == entity.Name);
                if (getProduct != null && !string.IsNullOrEmpty(getProduct.Name))
                {
                    return new Response(false, $"{entity.Name} already exists.");
                }
                var currentEntity = _context.Products.Add(entity).Entity;
                await _context.SaveChangesAsync();
                if (currentEntity != null && currentEntity.Id > 0)
                {
                    return new Response(true, $"{entity.Name} created successfully.");
                }
                else
                {
                    return new Response(false, $"Failed to create {entity.Name}.");
                }
            }
            catch (Exception ex)
            {
                // Log the orignal exception using the LogException class
                LogException.LogExceptions(ex);

                return new Response(false, "An error occurred while creating the product.");
            }
        }

        public async Task<Response> DeleteAsync(Product entity)
        {
            try
            {
                var prduct = await FindByIdAsync(entity.Id);
                if (prduct == null)
                {
                    return new Response(false, $"{entity.Name} does not exist.");
                }
                _context.Products.Remove(prduct);
                await _context.SaveChangesAsync();
                return new Response(true, $"{entity.Name} deleted successfully.");
            }
            catch (Exception ex)
            {
                // Log the orignal exception using the LogException class
                LogException.LogExceptions(ex);

                return new Response(false, "An error occurred while deleting the product.");
            }
        }

        public async Task<Product> FindByIdAsync(int id)
        {
            try
            {
                var product = await _context.Products.FindAsync(id);
                return product != null ? product : null!;
            }
            catch (Exception ex)
            {
                // Log the orignal exception using the LogException class
                LogException.LogExceptions(ex);
                throw new Exception("Error occurred while finding the product by ID.", ex);
            }
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            try
            {
                var products = await _context.Products.ToListAsync();
                return products != null ? products : null!;
            }
            catch (Exception ex)
            {
                // Log the orignal exception using the LogException class
                LogException.LogExceptions(ex);
                throw new Exception("Error occurred while retrieving all products.", ex);
            }
        }

        public async Task<Product> GetByAsync(Expression<Func<Product, bool>> predicate)
        {
            try
            {
                var product = await _context.Products.Where(predicate).FirstOrDefaultAsync()!;
                return product != null ? product : null!;
            }
            catch (Exception ex)
            {
                // Log the orignal exception using the LogException class
                LogException.LogExceptions(ex);

                throw new Exception("An error occurred while retrieving the product.", ex);
            }
        }

        public async Task<Response> UpdateAsync(Product entity)
        {
            try {
            var product = await FindByIdAsync(entity.Id);
                if (product == null)
                {
                    return new Response(false, $"{entity.Name} does not exist.");
                }
                _context.Entry(product).State = EntityState.Detached;
                _context.Products.Update(entity);
                await _context.SaveChangesAsync();
                return new Response(true, $"{entity.Name} updated successfully.");
            }
            catch (Exception ex)
            {
                // Log the orignal exception using the LogException class
                LogException.LogExceptions(ex);
                return new Response(false, "An error occurred while updating the product.");
            }
        }
    }
}
