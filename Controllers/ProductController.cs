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
            //var categoriesInDb = await _categoryRepository.GetAllAsync();
            //var brandsInDb = await _brandRepository.GetAllAsync();
            var viewModel = await PopulateModelAsync(new ProductFormViewModel());
            return View("ProductForm", viewModel);
        }


        public async Task<IActionResult> Edit(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return NotFound();

            var productInDb = await _productRepository.GetAsync(id);

            if(productInDb == null) return NotFound();

            var viewModel = new ProductFormViewModel(); 

            viewModel.Id = productInDb.Id;
            viewModel.Name = productInDb.Name;
            viewModel.Images = new List<IFormFile>();
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
                    if(model.Images.Count > 0)
                    {
                        var productImagesInDb = await _productImageRepository.GetAllAsync();

                        var productImages = productImagesInDb.Where(pimg => pimg.ProductId == model.Id).ToList();

                        string folderPath = Path.Combine(_hostingEnvironment.WebRootPath, "images\\products");

                        foreach (var productImage in productImages)
                        {
                            var filePath = Path.Combine(folderPath, productImage.ImageName);

                            System.IO.File.Delete(filePath);
                        }

                    }


                }
                else
                {
                    Product product = await AddProductAsync(model, user);

                    await AddAndUploadProductImagesAsync(model.Images, product, user);

                }

                await _productRepository.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            return View("ProductForm", model);
        }



        private async Task<ProductFormViewModel> PopulateModelAsync(ProductFormViewModel model)
        {
            var categoriesInDb = await _categoryRepository.GetAllAsync();
            var brandsInDb = await _brandRepository.GetAllAsync();

            model.PrimaryCategories = categoriesInDb.Where(c => c.CategoryType == Category.Primary).ToList();
            model.SecondaryCategories = model.PrimaryCategoryId == null ? new List<Category>() : categoriesInDb.Where(c => c.ParentId == model.PrimaryCategoryId).ToList();
            model.TertiaryCategories = model.SecondaryCategoryId == null ? new List<Category>() : categoriesInDb.Where(c => c.ParentId == model.SecondaryCategoryId).ToList();
            model.Brands = brandsInDb;

            return model;
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
                PrimaryCategoryId = model.PrimaryCategoryId,
                SecondaryCategoryId = model.SecondaryCategoryId,
                TertiaryCategoryId = model.TertiaryCategoryId,
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

    }
}
