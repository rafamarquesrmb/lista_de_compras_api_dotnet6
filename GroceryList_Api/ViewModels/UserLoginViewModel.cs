using System.ComponentModel.DataAnnotations;

namespace GroceryList_Api.ViewModels
{
    public class UserLoginViewModel
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Must be a valid Email")]
        public string Email { get; set;}
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set;}
    }
}
