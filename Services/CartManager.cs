using InventoryApp.Models;
using InventoryApp.Utility;

namespace InventoryApp.Services
{
    public class CartManager : ICartManager
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CartManager(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public List<Cart> AddToCart(Product product)
        {
            var cart = new Cart();

            cart.Id = product.Id;
            cart.Name = product.Name;
            cart.UnitPrice = product.Price;
            cart.Unit = product.Unit;            

            var cartItems = _httpContextAccessor.HttpContext.Session.Get<List<Cart>>("CartItems");

            if (cartItems == null)
                cartItems = new List<Cart>();

            bool isExist = cartItems.Find(c => c.Id == product.Id) != null ? true : false;

            if (!isExist)
            {
                cart.Quantity = 1;
                cart.ItemTotal = cart.Quantity * cart.UnitPrice;
                cartItems.Add(cart);
            }
            else
            {
                cartItems.Find(c => c.Id == product.Id).Quantity++;
                cartItems.Find(c => c.Id == product.Id).ItemTotal = cartItems.Find(c => c.Id == product.Id).Quantity * cart.UnitPrice;
            }

            var isSaved = SaveChanges(cartItems);

            return cartItems;
        }

        public List<Cart> GetCartItems()
        {
            var cartItems = _httpContextAccessor.HttpContext.Session.Get<List<Cart>>("CartItems");

            return cartItems == null ? null : cartItems;
        }

        public bool RemoveFromCart(string id)
        {
            var cartItems = _httpContextAccessor.HttpContext.Session.Get<List<Cart>>("CartItems");

            if(cartItems == null) cartItems = new List<Cart>();

            var item = cartItems.Find(c => c.Id == id);

            cartItems.Remove(item);

            var isSaved = SaveChanges(cartItems);

            return isSaved;
        }

        public bool IncreaseItemQuantity(string id)
        {
            var cartItems = _httpContextAccessor.HttpContext.Session.Get<List<Cart>>("CartItems");

            if (cartItems == null) return false;

            var product = cartItems.Find(c => c.Id == id);

            if (product == null) return false;

            cartItems.Find(c => c.Id == id).Quantity++;
            cartItems.Find(c => c.Id == id).ItemTotal += product.UnitPrice;

            var isIncreased = SaveChanges(cartItems);

            return isIncreased;
        }

        public bool DecreaseItemQuantity(string id)
        {
            var cartItems = _httpContextAccessor.HttpContext.Session.Get<List<Cart>>("CartItems");

            if (cartItems == null) return false;

            var product = cartItems.Find(c => c.Id == id);

            if (product == null) return false;

            cartItems.Find(c => c.Id == id).Quantity--;
            cartItems.Find(c => c.Id == id).ItemTotal -= product.UnitPrice;

            var isDecreased = SaveChanges(cartItems);

            return isDecreased;
        }


        public bool SaveChanges(List<Cart> cartItems)
        {
            try
            {
                _httpContextAccessor.HttpContext.Session.Set<List<Cart>>("CartItems", cartItems);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool ClearCart()
        {
            try
            {
                _httpContextAccessor.HttpContext.Session.SetString("CartItems", "");
                return true;
            }
            catch
            {
                return false;
            }
        }

        
    }
}
