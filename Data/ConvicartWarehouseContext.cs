using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ConvicartAdminApp.Models;
using System.Net;
using System.Reflection.Emit;

namespace ConvicartAdminApp.Data
{
    public class ConvicartWarehouseContext : IdentityDbContext
    {
        // DbSets for each table
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Preference> Preferences { get; set; }
        public DbSet<Store> Stores { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<QuerySubmission> QuerySubmissions { get; set; }
        public DbSet<RecipeSteps> RecipeSteps { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        // Constructor accepting DbContextOptions
        public ConvicartWarehouseContext(DbContextOptions<ConvicartWarehouseContext> options)
        : base(options) 
        {
            
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>()
                .Property(o => o.TotalAmount)
                .HasColumnType("decimal(18, 4)");  // Define a precision and scale that suits your needs

            modelBuilder.Entity<OrderItem>()
                .Property(oi => oi.Price)
                .HasColumnType("decimal(18, 4)");  // Adjust this to your requirements

            base.OnModelCreating(modelBuilder);
        }

    }

}
