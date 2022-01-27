using System.ComponentModel.DataAnnotations;

namespace GroceryList_Api.ViewModels
{
    public class AddItemViewModel
    {
        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; }
        public int Quantity { get; set; }
        public string QuantityTitle { get; set; }
        public bool Bought { get; set; }
    }
}
