using Microsoft.EntityFrameworkCore;
using OrderManagement.DomainLayer;

namespace OrderManagement.DataAccess
{
    public class OrderDbContext : DbContext
    {
        public OrderDbContext(DbContextOptions options) : base(options)
        {

        }
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer("Data Source = (localdb)\\mssqllocaldb; Initial Catalog = OrderDbV1; Integrated Security = true");
        //}

        public DbSet<Order> Orders { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
