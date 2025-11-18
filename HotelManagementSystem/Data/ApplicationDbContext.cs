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
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Driver> Drivers { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        
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