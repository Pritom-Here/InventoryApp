using System.ComponentModel.DataAnnotations;

namespace InventoryApp.Models
{
    public class Product : BaseEntity
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        
        [Required]
        public string Name { get; set; }
        
        public string Image { get; set; }
        
        [Required]
        public double Amount { get; set; }
        
        [Required]
        public string Unit { get; set; }
        
        [Required]
        public double Price { get; set; }

        [Required]
        public string Currency { get; set; }

        [Required]
        public string CategoryId { get; set; }

        public Category Category { get; set; }

        [Required]
        public string BrandId { get; set; }

        public Brand Brand { get; set; }

    }
}
