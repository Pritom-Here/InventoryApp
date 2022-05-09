using InventoryApp.Models;
using InventoryApp.Models.ViewModels;
using InventoryApp.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace InventoryApp.Controllers
{
    [Authorize(Roles = RolesAndPolicies.Roles.Administrator)]
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IAccountRepository _accountRepository;

        public CategoryController(ICategoryRepository categoryRepository, IAccountRepository accountRepository)
        {
            _categoryRepository = categoryRepository;
            _accountRepository = accountRepository;
        }

        public async Task<IActionResult> Index()
        {
            var categoriesInDb = await _categoryRepository.GetAllAsync();
            return View(categoriesInDb);
        }

        public async Task<IActionResult> Create()
        {
            var categoriesInDb = await _categoryRepository.GetAllAsync();
            var primaryCategories = categoriesInDb.Where(c => c.ParentId == null).ToList();

            var viewModel = new CategoryFormViewModel
            {
                PrimaryCategories = primaryCategories,
                SecondaryCategories = new List<Category>()
            };

            return View("CategoryForm", viewModel);
        }
        
        public async Task<IActionResult> Edit(string id)
        {
            if(string.IsNullOrWhiteSpace(id)) return NotFound();

            var categoryInDb = await _categoryRepository.GetAsync(id);

            if(categoryInDb == null) return NotFound();

            var categoriesInDb = await _categoryRepository.GetAllAsync();
            var primaryCategories = categoriesInDb.Where(c => c.ParentId == null).ToList();

            var secondaryCategory = categoryInDb.ParentId == null ? null : await _categoryRepository.GetAsync(categoryInDb.ParentId);
            

            var viewModel = new CategoryFormViewModel();

            viewModel.Id = categoryInDb.Id;
            viewModel.Name = categoryInDb.Name;
            viewModel.PrimaryCategoryId = categoryInDb.ParentId == null ? null : (secondaryCategory.ParentId == null ? secondaryCategory.Id : secondaryCategory.ParentId);
            viewModel.SecondaryCategoryId = categoryInDb.ParentId == null ? null : (secondaryCategory.ParentId == null ? null : secondaryCategory.Id);
            viewModel.PrimaryCategories = primaryCategories;
            viewModel.SecondaryCategories = categoriesInDb.Where(c => c.ParentId == viewModel.PrimaryCategoryId).ToList();
            
            return View("CategoryForm", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(CategoryFormViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _accountRepository.FindByNameAsync(User.Identity.Name);
                
                if(model.Id != null)
                {
                    var categoryInDb = await _categoryRepository.GetAsync(model.Id);

                    categoryInDb.Name = model.Name;
                    categoryInDb.ParentId = model.SecondaryCategoryId != null ? model.SecondaryCategoryId : model.PrimaryCategoryId;
                    categoryInDb.ModifiedOn = DateTime.Now;
                    categoryInDb.ModifiedBy = user.Id;
                }
                else
                {
                    var category = new Category();
                    category.Name = model.Name;
                    category.ParentId = model.SecondaryCategoryId != null ? model.SecondaryCategoryId : model.PrimaryCategoryId;
                    category.CreatedOn = DateTime.Now;
                    category.CreatedBy = user.Id;
                    category.ModifiedOn = category.CreatedOn;
                    category.ModifiedBy = category.CreatedBy;

                    await _categoryRepository.CreateAsync(category);
                }

                await _categoryRepository.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            var categoriesInDb = await _categoryRepository.GetAllAsync();
            var primaryCategories = categoriesInDb.Where(c => c.ParentId == null).ToList();

            model.PrimaryCategories = primaryCategories;
            model.SecondaryCategories = new List<Category>();
            return View(model);
        }


        [HttpDelete]
        public async Task<IActionResult> DeleteCategory(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return Json(new { status = 404, isDeleted = false, message = "Item not Found!" });

            var categoryInDb = await _categoryRepository.GetAsync(id);

            if(categoryInDb == null) return Json(new { status = 404, isDeleted = false, message = "Item not Found!" });

            categoryInDb.IsDeleted = true;

            var categoriesInDb = await _categoryRepository.GetAllAsync();
            var secondaryCategories = categoriesInDb.Where(c => c.ParentId == categoryInDb.Id).ToList();

            if(secondaryCategories.Count > 0)
            {
                foreach (var secondaryCategory in secondaryCategories)
                {
                    secondaryCategory.IsDeleted = true;

                    var tartiaryCategories = categoriesInDb.Where(c => c.ParentId == secondaryCategory.ParentId).ToList();

                    if(tartiaryCategories.Count > 0)
                    {
                        foreach (var tartiaryCategory in tartiaryCategories)
                        {
                            tartiaryCategory.IsDeleted = true;
                        }
                    }
                }
            }

            await _categoryRepository.SaveChangesAsync();

            return Json(new { status = 200, isDeleted = true, message = "Successfully deleted!" });
        }



        public async Task<IActionResult> GetChildCategories(string id)
        {
            if (id != null)
            {
                var categoriesInDb = await _categoryRepository.GetAllAsync();
                var secondaryCategories = categoriesInDb.Where(c => c.ParentId == id).ToList();
                
                return Json(new { data = secondaryCategories });
                //return Json(new { data = JsonSerializer.Serialize(secondaryCategories) });
            }

            return Json(null);
        }

    }
}
