using System.ComponentModel.DataAnnotations;

namespace InventoryApp.Models.ViewModels
{
    public class CreateUserViewModel
    {
        [Required]
        [StringLength(100, ErrorMessage = "The First Name field Should have a maximum of 100 characters")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The Last Name field Should have a maximum of 100 characters")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Please, Enter a valid email address")]
        public string Email { get; set; }

        public string Password { get; } = "AAaa11!!";
    }
}
