using InventoryApp.Models;
using InventoryApp.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace InventoryApp.Controllers
{
    public class ShopController : Controller
    {
        private readonly IProductRepository _productRepository;

        public ShopController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.ReturnUrl = Request.Headers["Referer"].ToString();

            IEnumerable<Product> products = await _productRepository.GetAllAsync();

            return View(products);
        }
    }
}
