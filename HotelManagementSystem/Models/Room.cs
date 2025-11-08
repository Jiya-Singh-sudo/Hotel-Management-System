using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace HotelManagementSystem.Models
{
    public class Room
    {
        [Key]
        public int RoomId { get; set; }

        [Required]
        [StringLength(50)]
        public string Type { get; set; } = string.Empty; // e.g., Deluxe, Suite, Standard

        [Required]
        public decimal PricePerNight { get; set; }

        public bool IsAvailable { get; set; } = true;

        // Optional: number of beds, AC/Non-AC, etc.
        public int Capacity { get; set; }

        // Navigation property
        // FIX 2: Initialized to a new List<Booking>() to satisfy CS8618
        public ICollection<Booking> Bookings { get; set; } = new List<Booking>(); 
    }
}