using System.ComponentModel.DataAnnotations;

namespace InventoryApp.Models
{
    public class Brand : BaseEntity
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        
        [Required]
        [StringLength(255)]
        public string Name { get; set; }
    }
}
