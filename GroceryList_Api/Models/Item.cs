using System.Text.Json.Serialization;

namespace GroceryList_Api.Models
{
    public class Item
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public int Quantity { get; set; } = 1;
        public string QuantityTitle { get; set; } = "un";
        public bool Bought { get; set; } = false;
        [JsonIgnore]
        public Guid GroceryListId { get; set; }
        [JsonIgnore]
        public virtual GroceryList GroceryList { get; set; }
        
    }
}
