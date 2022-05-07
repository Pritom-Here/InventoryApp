using InventoryApp.Models;

namespace InventoryApp.Repositories.Interfaces
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetAllAsync();
        Task<Category> GetAsync(string id);
        Task CreateAsync(Category category);
        Task SaveChangesAsync();
    }
}
