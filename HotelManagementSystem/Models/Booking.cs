using System;

namespace HotelManagementSystem.Models
{
    public class Booking
    {
        public int BookingId { get; set; }
        public int CustomerId { get; set; }
        public int RoomId { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public decimal TotalAmount { get; set; }
        
        // Optional Driver (suggested by 'AddDriverToBooking' migration)
        public int? DriverId { get; set; } 

        // Navigation properties
        public Customer? Customer { get; set; }
        public Room? Room { get; set; }
        public Driver? Driver { get; set; } // Nullable foreign key for optional driver
    }
}