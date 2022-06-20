using InventoryApp.Models;
using InventoryApp.Models.ViewModels;
using InventoryApp.Repositories.Interfaces;
using InventoryApp.Services;
using InventoryApp.Utility;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace InventoryApp.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly ICartManager _cartManager;

        public CheckoutController(ICartManager cartManager)
        {
            _cartManager = cartManager;
        }

        public IActionResult OutletCheckout()
        {
            var cartItems = _cartManager.GetCartItems();

            if(cartItems == null) cartItems = new List<Cart>();

            var model = new CheckoutViewModel();
            model.CartItems = cartItems;
            model.SubTotal = cartItems.Sum(c => c.ItemTotal);
            model.Total = model.SubTotal + model.SubTotal * (model.Vat / 100);

            return View(model);
        }

    }
}
