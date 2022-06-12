using InventoryApp.Models;
using InventoryApp.Models.ViewModels;
using InventoryApp.Repositories.Interfaces;
using InventoryApp.Utility;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace InventoryApp.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly IProductRepository _productRepository;

        public CheckoutController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public IActionResult OutletCheckout()
        {
            var cartItems = HttpContext.Session.Get<List<Cart>>("CartItems");

            if(cartItems == null) cartItems = new List<Cart>();

            var model = new CheckoutViewModel();
            model.CartItems = cartItems;
            model.SubTotal = cartItems.Sum(c => c.ItemTotal);
            model.Total = model.SubTotal + model.SubTotal * (model.Vat / 100);

            return View(model);
        }

    }
}
