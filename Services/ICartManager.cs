using InventoryApp.Models;

namespace InventoryApp.Services
{
    public interface ICartManager
    {
        List<Cart> AddToCart(Product product);
        List<Cart> GetCartItems();
        bool RemoveFromCart(string id);
        bool ClearCart();
        bool IncreaseItemQuantity(string id);
        bool DecreaseItemQuantity(string id);
    }
}
