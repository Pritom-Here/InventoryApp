using InventoryApp.Models;

namespace InventoryApp.Repositories.Interfaces
{
    public interface IBrandRepository
    {
        Task<IEnumerable<Brand>> GetAllAsync();
        Task<Brand> GetAsync(string id);
        Task CreateAsync(Brand brand);
    }
}
