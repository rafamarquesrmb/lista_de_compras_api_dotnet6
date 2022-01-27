namespace GroceryList_Api
{
    public static class Configuration
    {
        public static string JwtKey { get; set; } = "minhachavesecreta";
        public static string ConnectionString { get; set; } = "Server=localhost,1433;Database=Grocery_list;User ID=sa;Password=sa";
    }
}
