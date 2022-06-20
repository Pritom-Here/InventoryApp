using InventoryApp.Models;
using InventoryApp.Models.ViewModels;
using InventoryApp.Repositories.Interfaces;
using InventoryApp.Services;
using InventoryApp.Utility;
using Microsoft.AspNetCore.Mvc;

namespace InventoryApp.Controllers
{
    public class POSController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICartManager _cartManager;

        public POSController(IUnitOfWork unitOfWork, ICartManager cartManager)
        {
            _unitOfWork = unitOfWork;
            _cartManager = cartManager;
        }

        public async Task<IActionResult> Index()
        {
            var productsInDb = await _unitOfWork.Products.GetAllAsync();
            var categoriesInDb = await _unitOfWork.Categories.GetAllAsync();

            var cartItems = HttpContext.Session.Get<List<Cart>>("CartItems");
            if (cartItems == null) cartItems = new List<Cart>();

            var viewModel = new POSViewModel
            {
                PrimaryCategories = categoriesInDb.Where(c => c.CategoryType == Category.Primary).ToList(),
                Products = productsInDb,
                CartItemsCount = cartItems.Count()
            };

            return View(viewModel);
        }


        public async Task<IActionResult> FilterProducts(string id)
        {
            var productsInDb = await _unitOfWork.Products.GetAllAsync();
            var categoriesInDb = await _unitOfWork.Categories.GetAllAsync();

            var viewModel = new POSViewModel
            {
                SecondaryCategories = categoriesInDb.Where(c => c.ParentId == id && c.CategoryType == Category.Secondary).ToList(),
                TertiaryCategories = categoriesInDb.Where(c => c.ParentId == id && c.CategoryType == Category.Tertiary).ToList(),
                Products = productsInDb.Where(p => p.PrimaryCategoryId == id || p.SecondaryCategoryId == id || p.TertiaryCategoryId == id).ToList()

            };

            return Json(new { status = 200, data = viewModel });
        }
        

        [HttpPost]
        public async Task<IActionResult> SearchProducts(string search)
        {
            var productsInDb = await _unitOfWork.Products.GetAllAsync();

            var products = productsInDb.Where(p => p.Name.ToLower().Contains(search) || p.ProductCode.ToLower().Contains(search)).ToList();

            return Json(new { status = 200, data = products });
        }

    }
}
