using HotelManagementSystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;

namespace HotelManagementSystem.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            
        }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Driver> Drivers { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        
        // Optional: Fluent API configuration for relationships (e.g., making DriverId nullable)
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
             // Configure the DriverId in Booking to be optional (null)
            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Driver)
                .WithMany() // Assuming Driver doesn't have a collection of Bookings
                .HasForeignKey(b => b.DriverId)
                .IsRequired(false); // Allows the foreign key to be null

            base.OnModelCreating(modelBuilder);
        }
    }
}
