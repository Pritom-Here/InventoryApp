using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryApp.Models
{
    public class Product : BaseEntity
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        
        [Required]
        public string Name { get; set; }
        
        [Required]
        public string ProductCode { get; set; }
        
        [Required]
        public string Unit { get; set; }
        
        [Required]
        public double Price { get; set; }

        [Required]
        public string Currency { get; set; }
        
        [Required]
        public double InStock { get; set; }

        [Required]
        public double WarningLevel { get; set; }

        
        [Required]
        public string PrimaryCategoryId { get; set; }

        [ForeignKey("PrimaryCategoryId")]
        public Category PrimaryCategory { get; set; }
        
        
        public string SecondaryCategoryId { get; set; }

        [ForeignKey("SecondaryCategoryId")]
        public Category SecondaryCategory { get; set; }
        

        public string TertiaryCategoryId { get; set; }

        [ForeignKey("TertiaryCategoryId")]
        public Category TertiaryCategory { get; set; }

        
        [Required]
        public string BrandId { get; set; }

        public Brand Brand { get; set; }

    }
}
