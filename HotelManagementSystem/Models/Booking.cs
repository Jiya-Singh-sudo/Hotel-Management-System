using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelManagementSystem.Models
{
    public class Booking
    {
        [Key]
        public int BookingId { get; set; }

        // --- Foreign Keys ---
        
        [Required]
        public int CustomerId { get; set; }

        [Required]
        public int RoomId { get; set; }

        public int? DriverId { get; set; } // Optional (int? allows NULL)

        // --- Core Properties ---
        
        [Required]
        [DataType(DataType.Date)]
        public DateTime CheckInDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime CheckOutDate { get; set; }

        [Required]
        public int NumberOfGuests { get; set; }

        [Required]
        public decimal TotalAmount { get; set; }

        // --- Navigation Properties ---

        [ForeignKey("CustomerId")]
        // FIX 1: Required FK, so use the null-forgiving operator (= null!)
        public required Customer Customer { get; set; } = null!;

        [ForeignKey("RoomId")]
        // FIX 2: Required FK, so use the null-forgiving operator (= null!)
        public required Room Room { get; set; } = null!;

        [ForeignKey("DriverId")]
        // FIX 3: Optional FK (DriverId is int?), so the navigation property must be nullable (Driver?)
        public Driver? Driver { get; set; } 
    }
}