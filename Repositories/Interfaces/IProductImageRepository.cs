using InventoryApp.Models;

namespace InventoryApp.Repositories.Interfaces
{
    public interface IProductImageRepository
    {
        Task CreateAsync(ProductImage productImage);
        Task SaveChangesAsync();
    }
}
