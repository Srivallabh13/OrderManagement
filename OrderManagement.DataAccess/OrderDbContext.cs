using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OrderManagement.DomainLayer.DTO;
using OrderManagement.DomainLayer.Entities;

namespace OrderManagement.DataAccess
{
    public class OrderDbContext : IdentityDbContext<User>
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
        public DbSet<OrderProduct> OrderProducts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.User) //Each Order has one User.
                .WithMany(u => u.Orders) // Each User can have many Orders.
                .HasForeignKey(o => o.CustId)  //The foreign key on the Order entity is CustId.
                .HasPrincipalKey(o => o.Id) //The principal key on the User entity is Id.
                .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<OrderProduct>()
                .HasKey(op => new { op.Id, op.ProductId }); 

            modelBuilder.Entity<OrderProduct>()
                .HasOne(op => op.Order)
                .WithMany(o => o.Products)
                .HasForeignKey(op => op.Id)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
