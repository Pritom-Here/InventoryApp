using InventoryApp.Models.CustomValidations;
using System.ComponentModel.DataAnnotations;

namespace InventoryApp.Models.ViewModels
{
    public class ProductFormViewModel
    {

        public IEnumerable<Category> PrimaryCategories { get; set; }
        public IEnumerable<Category> SecondaryCategories { get; set; }
        public IEnumerable<Category> TertiaryCategories { get; set; }
        public IEnumerable<Brand> Brands { get; set; }


        public string Id { get; set; }

        [Required]
        public string Name { get; set; }

        //[Required]
        [DataType(DataType.Upload)]
        [NumberOfItemsInList(0, 4, ErrorMessage = "Number of images should not be more than 4")]
        [AllowedExtensions(new string[] { ".jpg", ".jpeg", ".png" })]
        public List<IFormFile> Images { get; set; } = new List<IFormFile>();

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
        [Display(Name = "Primary Category")]
        public string PrimaryCategoryId { get; set; }

        [Display(Name = "Secondary Category")]
        public string SecondaryCategoryId { get; set; }

        [Display(Name = "Tertiary Category")]
        public string TertiaryCategoryId { get; set; }

        
        [Required]
        [Display(Name = "Brand")]
        public string BrandId { get; set; }
    }
}
