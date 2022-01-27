namespace GroceryList_Api.ViewModels
{
    public class UpdateItemViewModel
    {
        public string? Title { get; set; }
        public int? Quantity { get; set; }
        public string? QuantityTitle { get; set; }
        public bool? Bought { get; set; }
    }
}
