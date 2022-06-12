using InventoryApp.Models;
using InventoryApp.Models.ViewModels;
using InventoryApp.Repositories.Interfaces;
using InventoryApp.Utility;
using Microsoft.AspNetCore.Mvc;

namespace InventoryApp.Controllers
{
    public class POSController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;

        public POSController(IProductRepository productRepository, ICategoryRepository categoryRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
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


        [HttpPost]
        public async Task<IActionResult> AddToCart(string id)
        {
            if(string.IsNullOrWhiteSpace(id)) return NotFound();

            var product = await _productRepository.GetAsync(id);
            
            if(product == null) return NotFound();

            var cart = new Cart();

            cart.Id = product.Id;
            cart.Name = product.Name;
            cart.UnitPrice = product.Price;
            cart.Unit = product.Unit;
            
            var cartItems = HttpContext.Session.Get<List<Cart>>("CartItems");

            if(cartItems == null)
                cartItems = new List<Cart>();

            bool isExist = cartItems.Find(c => c.Id == id) != null ? true : false;

            if (!isExist)
            {
                cart.Quantity = 1;
                cart.ItemTotal = cart.Quantity * cart.UnitPrice;
                cartItems.Add(cart);
            }
            else
            {
                cartItems.Find(c => c.Id == id).Quantity++;
                cartItems.Find(c => c.Id == id).ItemTotal = cartItems.Find(c => c.Id == id).Quantity * cart.UnitPrice;
            }

            HttpContext.Session.Set<List<Cart>>("CartItems", cartItems);


            // If Product is already in the cart then only quantity and item total will be updated
            // And a member called IsExist will be added to the returned Json response
            // Based on the IsExist value Cart Item Count will be updated in the dom
            return Json(new { status = 200, data = cartItems, message = "Successfully Added To Cart", isExist = isExist });
        }

    }
}
