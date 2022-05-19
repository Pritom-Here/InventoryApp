using InventoryApp.Data;
using InventoryApp.Models;
using InventoryApp.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace InventoryApp.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public ProductRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            var productsInDb = await _applicationDbContext.Products.Include(p => p.Category).Include(p => p.Brand).Include(p => p.Creator).Include(p => p.Modifier).ToListAsync();
            return productsInDb;
            //throw new NotImplementedException();

        }

        public async Task<Product> GetAsync(string id)
        {
            var productInDb = await _applicationDbContext.Products.Include(p => p.Category).Include(p => p.Brand).Include(p => p.Creator).Include(p => p.Modifier).FirstOrDefaultAsync(p => p.Id == id);
            return productInDb;
        }

        public async Task CreateAsync(Product product)
        {
            await _applicationDbContext.Products.AddAsync(product);
        }

        public async Task SaveChangesAsync()
        {
            await _applicationDbContext.SaveChangesAsync();
        }
    }
}
