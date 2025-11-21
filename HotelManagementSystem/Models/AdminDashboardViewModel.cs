using System.Collections.Generic;

namespace HotelManagementSystem.Models
{
    public class AdminDashboardViewModel
    {
        public int TotalBookings { get; set; }
        public int TotalCustomers { get; set; }
        public int OccupiedRooms { get; set; }
        public decimal TotalRevenue { get; set; }
        
        public List<Booking> RecentBookings { get; set; } = new List<Booking>();
    }
}