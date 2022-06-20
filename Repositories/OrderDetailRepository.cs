using InventoryApp.Data;
using InventoryApp.Models;
using InventoryApp.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace InventoryApp.Repositories
{
    public class OrderDetailRepository : IOrderDetailRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public OrderDetailRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task CreateAsync(OrderDetail orderDetail)
        {
            await _dbContext.OrderDetails.AddAsync(orderDetail);
        }

        public async Task<IEnumerable<OrderDetail>> GetAllAsync()
        {
            var orderDetailsInDb = await _dbContext.OrderDetails.Include(od => od.CreatedBy).Include(od => od.ModifiedBy).ToListAsync();
            return orderDetailsInDb;
        }

        public async Task<OrderDetail> GetAsync(string id)
        {
            var orderDetailInDb = await _dbContext.OrderDetails.Include(od => od.CreatedBy).Include(od => od.ModifiedBy).FirstOrDefaultAsync(od => od.Id == id);
            return orderDetailInDb;
        }
    }
}
