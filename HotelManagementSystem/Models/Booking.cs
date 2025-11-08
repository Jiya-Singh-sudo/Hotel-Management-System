using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelManagementSystem.Models
{
    public class Booking
    {
        [Key]
        public int BookingId { get; set; }

        [Required]
        public int CustomerId { get; set; }

        [ForeignKey("CustomerId")]
        public Customer Customer { get; set; } = null!;

        // 🛑 PROBLEM: This 'RoomType' string cannot replace the required Room link.
        // public string RoomType { get; set; } = null!; 
        
        // 🛠️ SOLUTION: Add the Foreign Key and Navigation Property

        [Required] // Assumes a booking MUST have a room
        public int RoomId { get; set; } 

        [ForeignKey("RoomId")]
        // The BookingsController is expecting this property name: 'Room'
        public Room Room { get; set; } = null!; 
        
        // -----------------------------------------------------------------

        public int NumberOfGuests { get; set; }

        [DataType(DataType.Date)]
        public DateTime CheckInDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime CheckOutDate { get; set; }

        public decimal TotalAmount { get; set; }

        public int? DriverId { get; set; }

        [ForeignKey("DriverId")]
        public Driver? Driver { get; set; }
    }
}