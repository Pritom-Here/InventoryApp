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
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ICartManager _cartManager;

        public POSController(IProductRepository productRepository, ICategoryRepository categoryRepository, ICartManager cartManager)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _cartManager = cartManager;
        }

        public async Task<IActionResult> Index()
        {
            var productsInDb = await _productRepository.GetAllAsync();
            var categoriesInDb = await _categoryRepository.GetAllAsync();

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
            var productsInDb = await _productRepository.GetAllAsync();
            var categoriesInDb = await _categoryRepository.GetAllAsync();

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
            var productsInDb = await _productRepository.GetAllAsync();

            var products = productsInDb.Where(p => p.Name.ToLower().Contains(search) || p.ProductCode.ToLower().Contains(search)).ToList();

            return Json(new { status = 200, data = products });
        }

    }
}
