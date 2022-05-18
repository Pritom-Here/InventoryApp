using System.ComponentModel.DataAnnotations;

namespace InventoryApp.Models
{
    public class ProductImage
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        
        [Required]
        public string ImageName { get; set; }

        [Required]
        public string ProductId { get; set; }
        
        public Product Product { get; set; }
    }
}
