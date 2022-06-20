using InventoryApp.Models;
using InventoryApp.Models.ViewModels;
using InventoryApp.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace InventoryApp.Controllers
{
    public class ShopController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public ShopController(IProductRepository productRepository, ICategoryRepository categoryRepository, IBrandRepository brandRepository, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.ReturnUrl = Request.Headers["Referer"].ToString();

            var categoriesInDb = await _unitOfWork.Categories.GetAllAsync();
            var productsInDb = await _unitOfWork.Products.GetAllAsync();
            var brandsInDb = await _unitOfWork.Brands.GetAllAsync();

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
