using InventoryApp.Data;
using InventoryApp.Repositories.Interfaces;

namespace InventoryApp.Repositories
{
    public class UnitOfWork: IUnitOfWork
    {
        private readonly ApplicationDbContext _dbContext;
        public IBrandRepository Brands { get; private set; }
        public ICategoryRepository Categories { get; private set; }
        public IProductRepository Products { get; private set; }
        public IProductImageRepository ProductImages { get; private set; }
        public IOrderRepository Orders { get; private set; }
        public IOrderDetailRepository OrderDetails { get; private set; }

        public UnitOfWork(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            Brands = new BrandRepository(_dbContext);
            Categories = new CategoryRepository(_dbContext);
            Products = new ProductRepository(_dbContext);
            ProductImages = new ProductImageRepository(_dbContext);
            Orders = new OrderRepository(_dbContext);
            OrderDetails = new OrderDetailRepository(_dbContext);
        }

        public async Task<int> CompleteAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}
