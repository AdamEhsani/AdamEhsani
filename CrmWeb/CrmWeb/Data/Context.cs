using CrmWeb.Pages.Clients;
using Microsoft.EntityFrameworkCore;
using CrmWeb.Models;
using Microsoft.Extensions.Options;

namespace CrmWeb.Data
{
    public class Context : DbContext
    {
        public DbSet<Product> Products { get; set; } = null!;
        public DbSet<Customer> Customers { get; set; } = null!;
        public DbSet<Models.Orders> Orders { get; set; } = null!;
        public DbSet<OrderDetail> OrderDetails { get; set; } = null!;
        public DbSet<Setting> Settings { get; set; } = null!;
        public DbSet<BillDetail> BillDetail { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=(LocalDB)\\MSSQLLocalDB;Initial Catalog=Context;Integrated Security=True;");
        }
    }

}
