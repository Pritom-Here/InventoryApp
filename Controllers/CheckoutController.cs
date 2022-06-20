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
        private readonly IRandomCodeGenerator _randomCodeGenerator;
        private readonly IUnitOfWork _unitOfWork;

        public CheckoutController(ICartManager cartManager, IRandomCodeGenerator randomCodeGenerator, IUnitOfWork unitOfWork)
        {
            _cartManager = cartManager;
            _randomCodeGenerator = randomCodeGenerator;
            _unitOfWork = unitOfWork;
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


        public async Task<IActionResult> ConfirmOrder(CheckoutViewModel model)
        {
            if (ModelState.IsValid)
            {
                Order order = new Order()
                {
                    TransactionCode = await GetTransactionCode(),
                    TransactionDate = DateTime.Now,
                    SubTotal = model.SubTotal,
                    VatInPercentage = model.Vat,
                    Total = model.Total,
                    PaidAmount = model.PaidAmount
                };

                await _unitOfWork.Orders.CreateAsync(order);

                var cartItems = _cartManager.GetCartItems();

                foreach (var item in cartItems)
                {
                    //Add In OrderDetails
                    var orderDetail = new OrderDetail
                    {
                        Quantity = item.Quantity,
                        ItemTotal = item.ItemTotal,
                        OrderId = order.Id,
                        ProductId = item.Id
                    };

                    await _unitOfWork.OrderDetails.CreateAsync(orderDetail);

                    //Deduct Product Quantity
                    var product = await _unitOfWork.Products.GetAsync(item.Id);
                    product.InStock = product.InStock - orderDetail.Quantity;
                }

                int affectedRows = await _unitOfWork.CompleteAsync();

                //Clear Cart
                if(affectedRows > 0) _cartManager.ClearCart();

                //Redirect To POS Page
                return RedirectToAction("Index", "POS");
            }

            return View("OutletCheckout", model);
        }


        private async Task<string> GetTransactionCode()
        {
            var transactionCode = "";

            while (true)
            {
                transactionCode = _randomCodeGenerator.GenerateRandomCode();
                var ordersInDb = await _unitOfWork.Orders.GetAllAsync();
                bool isCodeAvailable = ordersInDb.Any(x => x.TransactionCode == transactionCode);
                
                if (!isCodeAvailable) break;
            }

            return transactionCode;
        }

    }
}
