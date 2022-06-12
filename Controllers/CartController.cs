using InventoryApp.Models;
using InventoryApp.Utility;
using Microsoft.AspNetCore.Mvc;

namespace InventoryApp.Controllers
{
    public class CartController : Controller
    {
        public IActionResult ClearCart()
        {
            HttpContext.Session.SetString("CartItems", "");

            return Json(new { status = 200 });
        }


        [HttpPost]
        public IActionResult DeleteCartItem(string id)
        {
            var cartItems = HttpContext.Session.Get<List<Cart>>("CartItems");

            var item = cartItems.Find(c => c.Id == id);

            cartItems.Remove(item);

            HttpContext.Session.Set<List<Cart>>("CartItems", cartItems);

            return Json(new { status = 200 });
        }


        [HttpPost]
        public IActionResult DecreaseItemQuantity(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return NotFound();

            var cartItems = HttpContext.Session.Get<List<Cart>>("CartItems");

            if (cartItems == null) return NotFound();

            var product = cartItems.Find(c => c.Id == id);

            if (product == null) return NotFound();

            cartItems.Find(c => c.Id == id).Quantity--;
            cartItems.Find(c => c.Id == id).ItemTotal-= product.UnitPrice;

            HttpContext.Session.Set<List<Cart>>("CartItems", cartItems);

            return Json(new { status = 200});
            
        }
        
        
        [HttpPost]
        public IActionResult IncreaseItemQuantity(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return NotFound();

            var cartItems = HttpContext.Session.Get<List<Cart>>("CartItems");

            if (cartItems == null) return NotFound();

            var product = cartItems.Find(c => c.Id == id);

            if (product == null) return NotFound();

            cartItems.Find(c => c.Id == id).Quantity++;
            cartItems.Find(c => c.Id == id).ItemTotal+= product.UnitPrice;

            HttpContext.Session.Set<List<Cart>>("CartItems", cartItems);

            return Json(new { status = 200});
            
        }
    }
}
