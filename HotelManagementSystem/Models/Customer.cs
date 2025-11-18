namespace HotelManagementSystem.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Username { get; set; }
        // The migration 'AddPasswordToCustomer' suggests this field
        public string? PasswordHash { get; set; } 
        public bool IsAdmin { get; set; } = false;

        // Navigation property for bookings
        public ICollection<Booking>? Bookings { get; set; }
    }
}