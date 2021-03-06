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

        private readonly IAccountRepository _accountRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CategoryController(IAccountRepository accountRepository, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _accountRepository = accountRepository;
        }

        public async Task<IActionResult> Index()
        {
            var categoriesInDb = await _unitOfWork.Categories.GetAllAsync();
            return View(categoriesInDb);
        }

        public async Task<IActionResult> Create()
        {
            var categoriesInDb = await _unitOfWork.Categories.GetAllAsync();
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

            var categoryInDb = await _unitOfWork.Categories.GetAsync(id);

            if(categoryInDb == null) return NotFound();

            var categoriesInDb = await _unitOfWork.Categories.GetAllAsync();

            var viewModel = new CategoryFormViewModel();

            viewModel.Id = categoryInDb.Id;
            viewModel.Name = categoryInDb.Name;
            viewModel.PrimaryCategoryId = categoryInDb.CategoryType == Category.Tertiary ? categoryInDb.Parent.ParentId : categoryInDb.ParentId; // Else block: if secondary, then parentId. If primary, then parentId value null. Thus primaryCategory
            viewModel.SecondaryCategoryId = categoryInDb.CategoryType == Category.Tertiary ? categoryInDb.ParentId : null;
            viewModel.PrimaryCategories = categoriesInDb.Where(c => c.CategoryType == Category.Primary).ToList();
            viewModel.SecondaryCategories = categoriesInDb.Where(c => c.CategoryType == Category.Secondary && c.ParentId != null && c.ParentId == viewModel.PrimaryCategoryId).ToList();
            
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
                    var categoryInDb = await _unitOfWork.Categories.GetAsync(model.Id);

                    categoryInDb.Name = model.Name;
                    categoryInDb.ParentId = model.SecondaryCategoryId != null ? model.SecondaryCategoryId : model.PrimaryCategoryId;
                    categoryInDb.CategoryType = model.SecondaryCategoryId == null && model.PrimaryCategoryId == null 
                                                                                                                ? Category.Primary 
                                                                                                                : (model.SecondaryCategoryId == null 
                                                                                                                                                ? Category.Secondary : 
                                                                                                                                                Category.Tertiary);
                    categoryInDb.ModifiedOn = DateTime.Now;
                    categoryInDb.ModifiedBy = user.Id;
                }
                else
                {
                    var category = new Category();
                    category.Name = model.Name;
                    category.ParentId = model.SecondaryCategoryId != null ? model.SecondaryCategoryId : model.PrimaryCategoryId;
                    category.CategoryType = model.SecondaryCategoryId == null && model.PrimaryCategoryId == null
                                                                                                                ? Category.Primary
                                                                                                                : (model.SecondaryCategoryId == null
                                                                                                                                                ? Category.Secondary :
                                                                                                                                                Category.Tertiary);
                    category.CreatedOn = DateTime.Now;
                    category.CreatedBy = user.Id;
                    category.ModifiedOn = category.CreatedOn;
                    category.ModifiedBy = category.CreatedBy;

                    await _unitOfWork.Categories.CreateAsync(category);
                }

                await _unitOfWork.CompleteAsync();

                return RedirectToAction("Index");
            }

            var categoriesInDb = await _unitOfWork.Categories.GetAllAsync();
            var primaryCategories = categoriesInDb.Where(c => c.ParentId == null).ToList();

            model.PrimaryCategories = primaryCategories;
            model.SecondaryCategories = new List<Category>();
            return View(model);
        }


        [HttpDelete]
        public async Task<IActionResult> DeleteCategory(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return Json(new { status = 404, isDeleted = false, message = "Item not Found!" });

            var categoryInDb = await _unitOfWork.Categories.GetAsync(id);

            if(categoryInDb == null) return Json(new { status = 404, isDeleted = false, message = "Item not Found!" });

            categoryInDb.IsDeleted = true;

            var categoriesInDb = await _unitOfWork.Categories.GetAllAsync();
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

            await _unitOfWork.CompleteAsync();

            return Json(new { status = 200, isDeleted = true, message = "Successfully deleted!" });
        }



        public async Task<IActionResult> GetChildCategories(string id)
        {
            if (id != null)
            {
                var categoriesInDb = await _unitOfWork.Categories.GetAllAsync();
                var secondaryCategories = categoriesInDb.Where(c => c.ParentId == id).ToList();
                
                return Json(new { data = secondaryCategories });
                //return Json(new { data = JsonSerializer.Serialize(secondaryCategories) });
            }

            return Json(null);
        }

    }
}
