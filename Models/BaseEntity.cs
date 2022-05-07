using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryApp.Models
{
    public class BaseEntity
    {
        [Required]
        public DateTime CreatedOn { get; set; }
        
        [Required]
        public DateTime ModifiedOn { get; set; }
        
        [Required]
        public string CreatedBy { get; set; }
        
        [ForeignKey("CreatedBy")]
        public ApplicationUser Creator { get; set; }
        
        [Required]
        public string ModifiedBy { get; set; }

        [ForeignKey("ModifiedBy")]
        public ApplicationUser Modifier { get; set; }
        
    }
}
