using InventoryApp.Models;
using InventoryApp.Models.ViewModels;
using InventoryApp.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryApp.Controllers
{
    [Authorize(Roles = RolesAndPolicies.Roles.Administrator)]
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
        
        public async Task<IActionResult> Edit(string id)
        {
            if(string.IsNullOrWhiteSpace(id)) return NotFound();

            var brandInDb = await _brandRepository.GetAsync(id);

            if(brandInDb == null) return NotFound();

            var viewModel = new BrandFormViewModel
            {
                Id = brandInDb.Id,
                Name = brandInDb.Name
            };
            return View("BrandForm", viewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(BrandFormViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _accountRepository.FindByNameAsync(User.Identity.Name);

                if (!string.IsNullOrWhiteSpace(model.Id))
                {
                    var brandInDb = await _brandRepository.GetAsync(model.Id);

                    if(brandInDb == null)
                    {
                        ModelState.AddModelError("", "Brand Data Integrity Has Been Altered");
                        return View("BrandForm", model);
                    }

                    brandInDb.Id = model.Id;
                    brandInDb.Name = model.Name;
                }
                else
                {
                    var brand = new Brand
                    {
                        Name = model.Name,
                        CreatedOn = DateTime.Now,
                        CreatedBy = user.Id,
                        ModifiedOn = DateTime.Now,
                        ModifiedBy = user.Id
                    };

                    await _brandRepository.CreateAsync(brand);

                }

                await _brandRepository.SaveChangesAsync();

                return RedirectToAction("Index", "Brand");
            }

            return View("BrandForm", model);
        }


        [HttpDelete]
        public async Task<IActionResult> DeleteBrand(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return Json(new { status = 404, isDeleted = false, message = "Item not Found!" });

            var brandInDb = await _brandRepository.GetAsync(id);

            if (brandInDb == null) return Json(new { status = 404, isDeleted = false, message = "Item not Found!" });

            brandInDb.IsDeleted = true;

            await _brandRepository.SaveChangesAsync();

            // NOTE :: Products Delete On Brand Delete Not Implemented

            return Json(new { status = 200, isDeleted = true, message = "Successfully deleted!" });
        }


    }
}
