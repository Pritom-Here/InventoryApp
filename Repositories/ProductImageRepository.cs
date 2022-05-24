using InventoryApp.Data;
using InventoryApp.Models;
using InventoryApp.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace InventoryApp.Repositories
{
    public class ProductImageRepository : IProductImageRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public ProductImageRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
        
        public async Task<IEnumerable<ProductImage>> GetAllAsync()
        {
            return await _applicationDbContext.ProductImages.Include(img => img.Creator).Include(img => img.Modifier).ToListAsync();
        }
        
        public async Task CreateAsync(ProductImage productImage)
        {
            await _applicationDbContext.ProductImages.AddAsync(productImage);
        }

        public async Task SaveChangesAsync()
        {
            await _applicationDbContext.SaveChangesAsync();
        }
    }
}
