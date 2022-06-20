using InventoryApp.Models;

namespace InventoryApp.Repositories.Interfaces
{
    public interface IOrderRepository
    {
        Task<IEnumerable<Order>> GetAllAsync();
        Task<Order> GetAsync(string id);
        Task CreateAsync(Order order);
    }
}
