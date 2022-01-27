using System.ComponentModel.DataAnnotations;

namespace GroceryList_Api.ViewModels
{
    public class UserRegistrationViewModel
    {
        [Required(ErrorMessage ="Email is required")]
        [EmailAddress(ErrorMessage = "Must be a valid Email")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Password Confirmation is required")]
        public string ConfirmPassword { get; set; }
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }
    }
}
