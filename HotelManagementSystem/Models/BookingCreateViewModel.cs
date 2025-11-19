using System;
using System.ComponentModel.DataAnnotations;

namespace HotelManagementSystem.Models
{
    // View Model for the Booking Create form, handling string inputs
    public class BookingCreateViewModel
    {
        [Required(ErrorMessage = "Customer Name is required.")]
        [Display(Name = "Customer Name")]
        public required string CustomerName { get; set; } // Input for customer name

        [Required(ErrorMessage = "Room ID is required.")]
        public int RoomId { get; set; }
        
        public int? DriverId { get; set; }
        
        [Required]
        public DateTime CheckInDate { get; set; }
        [Required]
        public DateTime CheckOutDate { get; set; }
        [Required]
        public int NumberOfGuests { get; set; }
        [Required]
        public decimal TotalAmount { get; set; }
        
        // Note: Booking entity properties like Status and CreatedAt are handled by the controller
    }
}