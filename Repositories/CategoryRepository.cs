using InventoryApp.Data;
using InventoryApp.Models;
using InventoryApp.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace InventoryApp.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public CategoryRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            var categoriesInDb = await _applicationDbContext.Categories.Include(c => c.Parent).Include(c => c.Creator).Include(c => c.Modifier).ToListAsync();

            return categoriesInDb;
        }

        public async Task<Category> GetAsync(string id)
        {
            var categoryInDb = await _applicationDbContext.Categories.Include(c => c.Parent).Include(c => c.Creator).Include(c => c.Modifier).FirstOrDefaultAsync(c => c.Id == id);
            return categoryInDb;
        }


        public async Task CreateAsync(Category category)
        {
            await _applicationDbContext.Categories.AddAsync(category);
        }
    }
}
