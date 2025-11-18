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
        public DbSet<Contact> Contacts { get; set; }
        
        // Optional: Fluent API configuration for relationships (e.g., making DriverId nullable)
       public DbSet<Customer> Customer { get; set; }
        public DbSet<Room> Room { get; set; }
        public DbSet<Driver> Driver { get; set; }
        public DbSet<Booking> Booking { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
             // FIX 4: Use the correct primary key name, BookingId
             modelBuilder.Entity<Booking>()
                .HasKey(b => b.Id); 
            
            // FIX 5: Configure the optional Driver relationship
            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Driver) // Use the correct navigation property name, Driver
                .WithMany() 
                .HasForeignKey(b => b.DriverId)
                .IsRequired(false); 

            base.OnModelCreating(modelBuilder);
        }
    }
}