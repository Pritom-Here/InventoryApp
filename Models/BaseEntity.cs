using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryApp.Models
{
    public class BaseEntity
    {
        [Required]
        [ScaffoldColumn(false)]
        public DateTime CreatedOn { get; set; }
        
        [Required]
        [ScaffoldColumn(false)]
        public DateTime ModifiedOn { get; set; }
        
        [Required]
        [ScaffoldColumn(false)]
        public string CreatedBy { get; set; }
        
        [ForeignKey("CreatedBy")]
        public ApplicationUser Creator { get; set; }
        
        [Required]
        [ScaffoldColumn(false)]
        public string ModifiedBy { get; set; }

        [ForeignKey("ModifiedBy")]
        public ApplicationUser Modifier { get; set; }

        [ScaffoldColumn(false)]
        public bool IsDeleted { get; set; } = false;

    }
}
