namespace HotelManagementSystem.Models
{
    public class Room
    {
        public int Id { get; set; }
        public string RoomNumber { get; set; } = string.Empty;
        public string RoomType { get; set; } = string.Empty;
        public decimal PricePerNight { get; set; }
        public string Status { get; set; } = "Available"; // Available, Occupied, Maintenance
        // public string? Description { get; set; }
        
        // Navigation property
        public List<Booking>? Bookings { get; set; }
    }
}