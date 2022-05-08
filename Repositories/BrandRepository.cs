using InventoryApp.Data;
using InventoryApp.Models;
using InventoryApp.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace InventoryApp.Repositories
{
    public class BrandRepository : IBrandRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public BrandRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<IEnumerable<Brand>> GetAllAsync()
        {
            var brandsInDb = await _applicationDbContext.Brands.Include(b => b.Creator).Include(b => b.Modifier).ToListAsync();
            return brandsInDb;
        }

        public async Task<Brand> GetAsync(string id)
        {
            var brandInDb = await _applicationDbContext.Brands.Include(b => b.Creator).Include(b => b.Modifier).FirstOrDefaultAsync(b => b.Id == id);
            return brandInDb;
        }

        public async Task CreateAsync(Brand brand)
        {
            await _applicationDbContext.Brands.AddAsync(brand);
        }

        public async Task SaveChangesAsync()
        {
            await _applicationDbContext.SaveChangesAsync();
        }
    }
}
