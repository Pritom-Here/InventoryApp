using InventoryApp.Models;
using InventoryApp.Models.ViewModels;
using InventoryApp.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace InventoryApp.Controllers
{
    public class ShopController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IBrandRepository _brandRepository;

        public ShopController(IProductRepository productRepository, ICategoryRepository categoryRepository, IBrandRepository brandRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _brandRepository = brandRepository;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.ReturnUrl = Request.Headers["Referer"].ToString();

            var categoriesInDb = await _categoryRepository.GetAllAsync();
            var productsInDb = await _productRepository.GetAllAsync();
            var brandsInDb = await _brandRepository.GetAllAsync();

            var primaryCategories = categoriesInDb.Where(c => c.CategoryType == Category.Primary);
            var secondaryCategories = categoriesInDb.Where(c => c.CategoryType == Category.Secondary).OrderBy(c => c.ParentId).ThenBy(c => c.Name);
            var tertiaryCategories = categoriesInDb.Where(c => c.CategoryType == Category.Tertiary);

            var shopViewModel = new ShopViewModel()
            {
                PrimaryCategories = primaryCategories,
                SecondaryCategories = secondaryCategories,
                TertiaryCategories = tertiaryCategories,
                Products = productsInDb,
                Brands = brandsInDb
            };


            return View(shopViewModel);
        }
    }
}
