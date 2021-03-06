using System.ComponentModel.DataAnnotations;

namespace InventoryApp.Models.ViewModels
{
    public class CategoryFormViewModel
    {
        public string Id { get; set; }
        public List<Category> PrimaryCategories { get; set; }
        public List<Category> SecondaryCategories { get; set; }

        [Required]
        public string Name { get; set; }

        [Display(Name = "Primary Category (Optional)")]
        public string PrimaryCategoryId { get; set; }

        [Display(Name = "Secondary Category (Optional)")]
        public string SecondaryCategoryId { get; set; }
    }
}
