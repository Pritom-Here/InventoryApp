using Microsoft.AspNetCore.Mvc;

namespace InventoryApp.Controllers
{
    public class ProductController : Controller
    {

        public ProductController()
        {

        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
