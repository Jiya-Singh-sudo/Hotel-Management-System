using System.ComponentModel.DataAnnotations;

namespace HotelManagementSystem.Models
{
    public class Booking
    {
        public int Id { get; set; }
        
        [Required]
        public int CustomerId { get; set; }
        [Required]
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
        
        public string Status { get; set; } = "Pending"; // Pending, Confirmed, Cancelled
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public Customer? Customer { get; set; }
        public Room? Room { get; set; }
        public Driver? Driver { get; set; }
    }
}