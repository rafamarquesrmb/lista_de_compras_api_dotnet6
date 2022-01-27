using System.ComponentModel.DataAnnotations;

namespace GroceryList_Api.ViewModels
{
    public class UserChangePasswordViewModel
    {
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
        [Required(ErrorMessage = "New Password is required")]
        public string NewPassword { get; set; }
        [Required(ErrorMessage = "New Password Confirmation is required")]
        public string NewPasswordConfirmation { get; set; }

    }
}
