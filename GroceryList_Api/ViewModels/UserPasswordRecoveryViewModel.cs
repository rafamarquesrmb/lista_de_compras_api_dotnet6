using System.ComponentModel.DataAnnotations;

namespace GroceryList_Api.ViewModels
{
    public class UserPasswordRecoveryViewModel
    {
        [Required(ErrorMessage ="Email is required")]
        [EmailAddress]
        public string Email { get; set; }
    }
}
