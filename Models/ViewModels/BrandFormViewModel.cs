using System.ComponentModel.DataAnnotations;

namespace InventoryApp.Models.ViewModels
{
    public class BrandFormViewModel
    {
        public string Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; }
    }
}
