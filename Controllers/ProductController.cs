using InventoryApp.Models;
using InventoryApp.Models.ViewModels;
using InventoryApp.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace InventoryApp.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IBrandRepository _brandRepository;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public ProductController(IProductRepository productRepository, ICategoryRepository categoryRepository, IBrandRepository brandRepository, IWebHostEnvironment hostingEnvironment)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _brandRepository = brandRepository;
            _hostingEnvironment = hostingEnvironment;
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
            if (ModelState.IsValid)
            {
                if(model.Id != null)
                {

                }
                else
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
                        BrandId = model.BrandId
                    };

                    await _productRepository.CreateAsync(product);

                    if(model.Images != null && model.Images.Count > 0)
                    {
                        string uniqueFileName = "";

                        foreach (var image in model.Images)
                        {
                            string folderPath = Path.Combine(_hostingEnvironment.WebRootPath, "images/products");
                            uniqueFileName = Guid.NewGuid().ToString() + "_" + image.FileName;
                            string filePath = Path.Combine(folderPath, uniqueFileName);
                            image.CopyTo(new FileStream(filePath, FileMode.Create));

                            var ProductImage = new ProductImage
                            {
                                ImageName = uniqueFileName,
                                ProductId = product.Id
                            };

                            //ProductImage Repository
                            //ProductImage Create

                        }
                    }
                }

                await _productRepository.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            var categoriesInDb = await _categoryRepository.GetAllAsync();
            var brandsInDb = await _brandRepository.GetAllAsync();

            model.Categories = categoriesInDb.Where(c => c.CategoryType == Category.Tertiary).ToList();
            model.Brands = brandsInDb;

            return View(model);
        }


    }
}
