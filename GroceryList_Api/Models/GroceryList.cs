using System.Text.Json.Serialization;

namespace GroceryList_Api.Models
{
    public class GroceryList
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public Guid UserId { get; set; }
        [JsonIgnore]
        public virtual User User { get; set; }
        
        public virtual List<Item> Items { get; set; } = new List<Item>();
    }
}
