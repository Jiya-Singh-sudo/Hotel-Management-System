namespace HotelManagementSystem.Models
{
    public class Driver
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? LicenseNumber { get; set; }
        public string? Phone { get; set; }
        public bool IsAvailable { get; set; } = true;
    }
}