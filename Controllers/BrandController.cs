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
        private readonly IAccountRepository _accountRepository;
        private readonly IUnitOfWork _unitOfWork;

        public BrandController(IAccountRepository accountRepository, IUnitOfWork unitOfWork)
        {
            _accountRepository = accountRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index()
        {
            var categoriesInDb = await _unitOfWork.Brands.GetAllAsync();
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

            var brandInDb = await _unitOfWork.Brands.GetAsync(id);

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
                    var brandInDb = await _unitOfWork.Brands.GetAsync(model.Id);

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

                    await _unitOfWork.Brands.CreateAsync(brand);

                }

                await _unitOfWork.CompleteAsync();

                return RedirectToAction("Index", "Brand");
            }

            return View("BrandForm", model);
        }


        [HttpDelete]
        public async Task<IActionResult> DeleteBrand(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return Json(new { status = 404, isDeleted = false, message = "Item not Found!" });

            var brandInDb = await _unitOfWork.Brands.GetAsync(id);

            if (brandInDb == null) return Json(new { status = 404, isDeleted = false, message = "Item not Found!" });

            brandInDb.IsDeleted = true;

            await _unitOfWork.CompleteAsync();

            // NOTE :: Products Delete On Brand Delete Not Implemented

            return Json(new { status = 200, isDeleted = true, message = "Successfully deleted!" });
        }


    }
}
