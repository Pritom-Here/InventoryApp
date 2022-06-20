using InventoryApp.Models;
using InventoryApp.Repositories.Interfaces;
using InventoryApp.Services;
using InventoryApp.Utility;
using Microsoft.AspNetCore.Mvc;

namespace InventoryApp.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartManager _cartManager;
        private readonly IUnitOfWork _unitOfWork;

        public CartController(ICartManager cartManager, IUnitOfWork unitOfWork)
        {
            _cartManager = cartManager;
            _unitOfWork = unitOfWork;
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return NotFound();

            var product = await _unitOfWork.Products.GetAsync(id);

            if (product == null) return NotFound();

            var cartItems = _cartManager.AddToCart(product);

            // If Product is already in the cart then only quantity and item total will be updated
            // And a member called IsExist will be added to the returned Json response
            // Based on the IsExist value Cart Item Count will be updated in the dom
            return Json(new { status = 200, count = cartItems.Count, message = "Successfully Added To Cart" });
        }

        public IActionResult ClearCart()
        {
            var isCleared = _cartManager.ClearCart();

            var status = isCleared ? 200 : 500;

            return Json(new { status });
        }


        [HttpPost]
        public IActionResult DeleteCartItem(string id)
        {
            var isRemoved = _cartManager.RemoveFromCart(id);

            var status = isRemoved ? 200 : 500;

            return Json(new { status });
        }


        [HttpPost]
        public IActionResult DecreaseItemQuantity(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return NotFound();

            var isDecreased = _cartManager.DecreaseItemQuantity(id);

            var status = isDecreased ? 200 : 500;

            return Json(new { status });
            
        }
        
        
        [HttpPost]
        public IActionResult IncreaseItemQuantity(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return NotFound();

            var isIncreased = _cartManager.IncreaseItemQuantity(id);

            var status = isIncreased ? 200 : 500;

            return Json(new { status });
            
        }
    }
}
