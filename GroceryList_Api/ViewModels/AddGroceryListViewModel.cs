using System.ComponentModel.DataAnnotations;

namespace GroceryList_Api.ViewModels
{
    public class AddGroceryListViewModel
    {
        [Required(ErrorMessage="Title is required")]
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
