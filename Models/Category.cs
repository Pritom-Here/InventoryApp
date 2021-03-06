using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryApp.Models
{
    public class Category : BaseEntity
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        public byte CategoryType { get; set; }

        public string ParentId { get; set; }

        [ForeignKey("ParentId")]
        public Category Parent { get; set; }

        public List<Category> ChildCategories { get; set; }


        public static readonly byte Primary = 1;
        public static readonly byte Secondary = 2;
        public static readonly byte Tertiary = 3;
    }
}
