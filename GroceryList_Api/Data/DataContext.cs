using GroceryList_Api.Models;
using Microsoft.EntityFrameworkCore;


namespace GroceryList_Api.Data
{
    public class DataContext :DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder options) =>
            options.UseSqlServer(Configuration.ConnectionString);

        public DbSet<User> Users { get; set; }
        public DbSet<GroceryList> GroceryLists { get; set; }
        public DbSet<Item> Items { get; set; }

    }
}
