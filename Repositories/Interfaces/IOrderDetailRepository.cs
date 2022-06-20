using InventoryApp.Models;

namespace InventoryApp.Repositories.Interfaces
{
    public interface IOrderDetailRepository
    {
        Task<IEnumerable<OrderDetail>> GetAllAsync();
        Task<OrderDetail> GetAsync(string id);
        Task CreateAsync(OrderDetail orderDetail);
    }
}
