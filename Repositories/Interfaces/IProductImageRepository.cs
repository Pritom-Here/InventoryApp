using InventoryApp.Models;

namespace InventoryApp.Repositories.Interfaces
{
    public interface IProductImageRepository
    {
        Task<IEnumerable<ProductImage>> GetAllAsync();
        Task CreateAsync(ProductImage productImage);
    }
}
