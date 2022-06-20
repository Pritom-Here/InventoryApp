using InventoryApp.Models;
using InventoryApp.Models.ViewModels;
using InventoryApp.Repositories.Interfaces;
using InventoryApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryApp.Controllers
{
    [Authorize(Roles = RolesAndPolicies.Roles.Administrator)]
    public class ProductController : Controller
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IAccountRepository _accountRepository;
        private readonly IRandomCodeGenerator _randomCodeGenerator;
        private readonly IUnitOfWork _unitOfWork;

        public ProductController(
            IWebHostEnvironment hostingEnvironment,
            IAccountRepository accountRepository,
            IRandomCodeGenerator randomCodeGenerator,
            IUnitOfWork unitOfWork
        )
        {
            _hostingEnvironment = hostingEnvironment;
            _accountRepository = accountRepository;
            _randomCodeGenerator = randomCodeGenerator;
            _unitOfWork = unitOfWork;
        }


        public async Task<IActionResult> Index()
        {
            var productsInDb = await _unitOfWork.Products.GetAllAsync();

            return View(productsInDb);
        }


        public async Task<IActionResult> Create()
        {
            //var categoriesInDb = await _categoryRepository.GetAllAsync();
            //var brandsInDb = await _brandRepository.GetAllAsync();
            var viewModel = await PopulateModelAsync(new ProductFormViewModel());
            return View("ProductForm", viewModel);
        }


        public async Task<IActionResult> Edit(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return NotFound();

            var productInDb = await _unitOfWork.Products.GetAsync(id);

            if(productInDb == null) return NotFound();

            var viewModel = new ProductFormViewModel(); 

            viewModel.Id = productInDb.Id;
            viewModel.Name = productInDb.Name;
            //viewModel.Images = new List<IFormFile>();
            viewModel.Unit = productInDb.Unit;
            viewModel.Price = productInDb.Price;
            viewModel.Currency = productInDb.Currency;
            viewModel.InStock = productInDb.InStock;
            viewModel.WarningLevel = productInDb.WarningLevel;
            viewModel.PrimaryCategoryId = productInDb.PrimaryCategoryId;
            viewModel.SecondaryCategoryId = productInDb.SecondaryCategoryId;
            viewModel.TertiaryCategoryId = productInDb.TertiaryCategoryId;
            viewModel.BrandId = productInDb.BrandId;

            viewModel = await PopulateModelAsync(viewModel);
            
            return View("ProductForm", viewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(ProductFormViewModel model)
        {
            model = await PopulateModelAsync(model);

            if (ModelState.IsValid)
            {
                var user = await _accountRepository.FindByNameAsync(User.Identity.Name);

                if(model.Id != null)
                {
                    await DeletePreviousProductImages(model);
                }

                Product product = await AddOrUpdateProductAsync(model, user);

                await AddAndUploadProductImagesAsync(model.Images, product, user);

                await _unitOfWork.CompleteAsync();

                return RedirectToAction("Index");
            }

            return View("ProductForm", model);
        }

        private async Task<ProductFormViewModel> PopulateModelAsync(ProductFormViewModel model)
        {
            var categoriesInDb = await _unitOfWork.Categories.GetAllAsync();
            var brandsInDb = await _unitOfWork.Brands.GetAllAsync();

            model.PrimaryCategories = categoriesInDb.Where(c => c.CategoryType == Category.Primary).ToList();
            model.SecondaryCategories = model.PrimaryCategoryId == null ? new List<Category>() : categoriesInDb.Where(c => c.ParentId == model.PrimaryCategoryId).ToList();
            model.TertiaryCategories = model.SecondaryCategoryId == null ? new List<Category>() : categoriesInDb.Where(c => c.ParentId == model.SecondaryCategoryId).ToList();
            model.Brands = brandsInDb;

            //if (model.Id != null)
            //{
            //    if (model.Images.Count == 0)
            //    {
            //        var productImagesInDb = await _productImageRepository.GetAllAsync();
            //        var images = productImagesInDb.Where(pimg => pimg.ProductId == model.Id && !pimg.IsDeleted).ToList();

            //        var folderPath = Path.Combine(_hostingEnvironment.WebRootPath, "images\\products");

            //        foreach (var img in images)
            //        {
            //            var filePath = Path.Combine(folderPath, img.ImageName);
            //            var stream = System.IO.File.OpenRead(filePath);

            //            model.Images.Add(new FormFile(stream, 0, stream.Length, null, Path.GetFileName(stream.Name)));
            //        }
            //    }
            //}

            return model;
        }


        private async Task<Product> AddOrUpdateProductAsync(ProductFormViewModel model, ApplicationUser user)
        {
            Product product = new Product();

            if (model.Id != null)
            {
                product = await _unitOfWork.Products.GetAsync(model.Id);
            }

            product.Name = model.Name;
            product.Unit = model.Unit;
            product.Price = model.Price;
            product.Currency = model.Currency;
            product.InStock = model.InStock;
            product.WarningLevel = model.WarningLevel;
            product.PrimaryCategoryId = model.PrimaryCategoryId;
            product.SecondaryCategoryId = model.SecondaryCategoryId;
            product.TertiaryCategoryId = model.TertiaryCategoryId;
            product.BrandId = model.BrandId;
            product.ModifiedBy = user.Id;
            product.ModifiedOn = DateTime.Now;

            if (model.Id == null)
            {
                product.ProductCode = await GetProductCode();
                product.CreatedBy = user.Id;
                product.CreatedOn = DateTime.Now;

                await _unitOfWork.Products.CreateAsync(product);
            }

            return product;
        }

        private async Task<string> GetProductCode()
        {
            var productCode = "";

            while (true)
            {
                productCode = _randomCodeGenerator.GenerateRandomCode();
                var product = await _unitOfWork.Products.GetByProductCodeAsync(productCode);
                if (product == null) break;
            }

            return productCode;
        }

        private async Task AddAndUploadProductImagesAsync(List<IFormFile> productImages, Product product, ApplicationUser user)
        {
            string uniqueFileName = "";

            foreach (var image in productImages)
            {
                string folderPath = Path.Combine(_hostingEnvironment.WebRootPath, "images\\products");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + image.FileName;
                string filePath = Path.Combine(folderPath, uniqueFileName);
                image.CopyTo(new FileStream(filePath, FileMode.Create));

                var productImage = new ProductImage
                {
                    ImageName = uniqueFileName,
                    ProductId = product.Id,
                    CreatedBy = user.Id,
                    CreatedOn = DateTime.Now,
                    ModifiedBy = user.Id,
                    ModifiedOn = DateTime.Now
                };

                //ProductImage Create
                await _unitOfWork.ProductImages.CreateAsync(productImage);

            }
        }

        private async Task DeletePreviousProductImages(ProductFormViewModel model)
        {
            if (model.Images.Count > 0)
            {
                var productImagesInDb = await _unitOfWork.ProductImages.GetAllAsync();

                var productImages = productImagesInDb.Where(pimg => pimg.ProductId == model.Id).ToList();

                //string folderPath = Path.Combine(_hostingEnvironment.WebRootPath, "images\\products");

                foreach (var productImage in productImages)
                {
                    productImage.IsDeleted = true;
                }

            }
        }

    }
}
