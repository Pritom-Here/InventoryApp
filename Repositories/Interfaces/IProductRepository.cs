﻿using InventoryApp.Models;

namespace InventoryApp.Repositories.Interfaces
{
    public interface IProductRepository
    {

        Task<IEnumerable<Product>> GetAllAsync();
        Task<Product> GetAsync(string id);
        Task CreateAsync(Product product);
        Task SaveChangesAsync();
    }
}