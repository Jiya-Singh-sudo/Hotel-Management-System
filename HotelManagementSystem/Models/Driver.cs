using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace HotelManagementSystem.Models
{
    public class Driver
    {
        [Key]
        public int DriverId { get; set; }

        [Required]
        [StringLength(100)]
        // FIX 1: Initialized non-nullable string
        public required string Name { get; set; } = string.Empty; 

        [Phone]
        // FIX 2: Initialized non-nullable string
        public required string Phone { get; set; } = string.Empty;

        [Required]
        // FIX 3: Initialized non-nullable string
        public required string LicenseNumber { get; set; } = string.Empty;

        public bool IsAvailable { get; set; } = true;

        // Navigation property
        // FIX 4: Initialized non-nullable collection
        public ICollection<Booking> Bookings { get; set; } = new List<Booking>(); 
    }
}