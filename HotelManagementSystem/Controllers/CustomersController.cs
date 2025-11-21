using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HotelManagementSystem.Data;
using HotelManagementSystem.Models;

namespace HotelManagementSystem.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CustomersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CustomersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Customers/Dashboard
        public async Task<IActionResult> Dashboard()
        {
            var today = DateTime.Today;

            // 1. Fetch Statistics
            var viewModel = new AdminDashboardViewModel
            {
                TotalBookings = await _context.Bookings.CountAsync(),
                
                TotalCustomers = await _context.Customers.CountAsync(),
                
                // Count rooms that are currently occupied (CheckIn <= Today < CheckOut)
                OccupiedRooms = await _context.Bookings
                    .Where(b => b.CheckInDate <= today && b.CheckOutDate > today)
                    .CountAsync(),

                // Sum of all booking amounts
                TotalRevenue = await _context.Bookings.SumAsync(b => b.TotalAmount),

                // 2. Fetch Recent Bookings (Last 5)
                RecentBookings = await _context.Bookings
                    .Include(b => b.Customer)
                    .Include(b => b.Room)
                    .OrderByDescending(b => b.Id) // Newest first
                    .Take(5)
                    .ToListAsync()
            };

            return View(viewModel);
        }
    }
}