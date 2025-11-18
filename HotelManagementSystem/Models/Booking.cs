namespace HotelManagementSystem.Models
{
    public class Booking
    {
        public int Id { get; set; }
        
        public int? CustomerId { get; set; }
        public int? RoomId { get; set; }
        public int? DriverId { get; set; }
        
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public int NumberOfGuests { get; set; }
        public decimal TotalAmount { get; set; }
        
        public string Status { get; set; } = "Pending"; // Pending, Confirmed, Cancelled
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public Customer? Customer { get; set; }
        public Room? Room { get; set; }
        public Driver? Driver { get; set; }
    }
}