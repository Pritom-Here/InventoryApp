using InventoryApp.Models;
using InventoryApp.Models.ViewModels;
using InventoryApp.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace InventoryApp.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly IProductImageRepository _productImageRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IBrandRepository _brandRepository;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IAccountRepository _accountRepository;

        public ProductController(
            IProductRepository productRepository, 
            IProductImageRepository productImageRepository,
            ICategoryRepository categoryRepository, 
            IBrandRepository brandRepository, 
            IWebHostEnvironment hostingEnvironment,
            IAccountRepository accountRepository
        )
        {
            _productRepository = productRepository;
            _productImageRepository = productImageRepository;
            _categoryRepository = categoryRepository;
            _brandRepository = brandRepository;
            _hostingEnvironment = hostingEnvironment;
            _accountRepository = accountRepository;
        }

        public async Task<IActionResult> Index()
        {
            var productsInDb = await _productRepository.GetAllAsync();
            return View(productsInDb);
        }

        public async Task<IActionResult> Create()
        {
            var categoriesInDb = await _categoryRepository.GetAllAsync();
            var brandsInDb = await _brandRepository.GetAllAsync();

            var viewModel = new ProductFormViewModel
            {
                Categories = categoriesInDb.Where(c => c.CategoryType == Category.Tertiary).ToList(),
                Brands = brandsInDb
            };
            return View("ProductForm", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(ProductFormViewModel model)
        {

            var categoriesInDb = await _categoryRepository.GetAllAsync();
            var brandsInDb = await _brandRepository.GetAllAsync();

            model.Categories = categoriesInDb.Where(c => c.CategoryType == Category.Tertiary).ToList();
            model.Brands = brandsInDb;

            if (ModelState.IsValid)
            {
                var user = await _accountRepository.FindByNameAsync(User.Identity.Name);

                if(model.Id != null)
                {

                }
                else
                {
                    Product product = await AddProductAsync(model, user);

                    if (model.Images != null && model.Images.Count > 0)
                    {
                        if (model.Images.Count != 4)
                        {
                            ModelState.AddModelError("", "Number of images should not be less or more than 4");
                            return View("ProductForm", model);
                        }

                        var isImageTypeValid = ValidateFileExtension(model.Images);

                        if (!isImageTypeValid)
                        {
                            ModelState.AddModelError("", "Invalid file type. Only images with .jpg/.jpeg/.png extension are allowed");
                            return View("ProductForm", model);
                        }

                        await AddAndUploadProductImagesAsync(model.Images, product, user);
                    }
                }

                await _productRepository.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            return View("ProductForm", model);
        }

        private async Task<Product> AddProductAsync(ProductFormViewModel model, ApplicationUser user)
        {
            var product = new Product
            {
                Name = model.Name,
                Unit = model.Unit,
                Price = model.Price,
                Currency = model.Currency,
                InStock = model.InStock,
                WarningLevel = model.WarningLevel,
                CategoryId = model.CategoryId,
                BrandId = model.BrandId,
                CreatedBy = user.Id,
                CreatedOn = DateTime.Now,
                ModifiedBy = user.Id,
                ModifiedOn = DateTime.Now
            };

            await _productRepository.CreateAsync(product);
            return product;
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
                await _productImageRepository.CreateAsync(productImage);

            }
        }

        private bool ValidateFileExtension(List<IFormFile> images)
        {
            if (images.Count != 4) return false;

            foreach (var image in images)
            {
                string[] allowedExtensions = { ".jpg", ".jpeg", ".png" };
                string fileExtension = Path.GetExtension(image.FileName).ToLower();

                if (!allowedExtensions.Contains(fileExtension))
                {
                    return false;
                }
            }

            return true;
        }

    }
}
