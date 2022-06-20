using InventoryApp.Data;
using InventoryApp.Models;
using InventoryApp.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace InventoryApp.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public OrderRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        public async Task CreateAsync(Order order)
        {
            await _dbContext.Orders.AddAsync(order);
        }

        public async Task<IEnumerable<Order>> GetAllAsync()
        {
            var ordersInDb = await _dbContext.Orders.Include(o => o.CreatedBy).Include(o => o.ModifiedBy).ToListAsync();
            return ordersInDb;
        }

        public async Task<Order> GetAsync(string id)
        {
            var orderInDb = await _dbContext.Orders.Include(o => o.CreatedBy).Include(o => o.ModifiedBy).FirstOrDefaultAsync(o => o.Id == id);
            return orderInDb;
        }
    }
}
