using InventoryApp.Models;
using InventoryApp.Models.ViewModels;
using InventoryApp.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace InventoryApp.Controllers
{
    public class BrandController : Controller
    {
        private readonly IBrandRepository _brandRepository;
        private readonly IAccountRepository _accountRepository;

        public BrandController(IBrandRepository brandRepository, IAccountRepository accountRepository)
        {
            _brandRepository = brandRepository;
            _accountRepository = accountRepository;
        }

        public async Task<IActionResult> Index()
        {
            var categoriesInDb = await _brandRepository.GetAllAsync();
            return View(categoriesInDb);
        }


        public IActionResult Create()
        {
            var viewModel = new BrandFormViewModel();
            return View("BrandForm", viewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(BrandFormViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _accountRepository.FindByNameAsync(User.Identity.Name);

                var brand = new Brand
                {
                    Name = model.Name,
                    CreatedOn = DateTime.Now,
                    CreatedBy = user.Id,
                    ModifiedOn = DateTime.Now,
                    ModifiedBy = user.Id
                };

                await _brandRepository.CreateAsync(brand);
                await _brandRepository.SaveChangesAsync();

                return RedirectToAction("Index", "Brand");
            }

            return View(model);
        }
    }
}
