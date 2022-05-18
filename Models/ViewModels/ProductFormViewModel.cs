using System.ComponentModel.DataAnnotations;

namespace InventoryApp.Models.ViewModels
{
    public class ProductFormViewModel
    {
        public string Id { get; set; }

        public IEnumerable<Category> Categories { get; set; }
        public IEnumerable<Brand> Brands { get; set; }



        [Required]
        public string Name { get; set; }

        [Required]
        public List<IFormFile> Images { get; set; }

        [Required]
        public string Unit { get; set; }

        [Required]
        public double Price { get; set; }

        [Required]
        public string Currency { get; set; }

        [Required]
        [Display(Name = "In Stock")]
        public double InStock { get; set; }

        [Required]
        [Display(Name = "Warning Level")]
        public double WarningLevel { get; set; }

        [Required]
        [Display(Name = "Category")]
        public string CategoryId { get; set; }

        [Required]
        [Display(Name = "Brand")]
        public string BrandId { get; set; }
    }
}
