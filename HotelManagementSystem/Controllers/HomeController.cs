using HotelManagementSystem.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace HotelManagementSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Home or /Home/Index - Main landing page
        public async Task<IActionResult> Index()
        {
            // Get available rooms count for display
            var availableRoomsCount = await _context.Rooms
                .CountAsync(r => r.Status == "Available");

            // Get total bookings count (optional - for stats)
            var totalBookingsCount = await _context.Bookings.CountAsync();

            // Pass data to view
            ViewBag.AvailableRoomsCount = availableRoomsCount;
            ViewBag.TotalBookings = totalBookingsCount;

            // Check if user is logged in
            if (User.Identity?.IsAuthenticated == true)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var userName = User.FindFirstValue(ClaimTypes.Name);
                ViewBag.UserName = userName;
                ViewBag.IsAuthenticated = true;
            }
            else
            {
                ViewBag.IsAuthenticated = false;
            }

            return View();
        }

        // GET: /Home/About - About page (optional)
        public IActionResult About()
        {
            return View();
        }

        // GET: /Home/Contact - Contact page (optional)
        public IActionResult Contact()
        {
            return View();
        }

        // GET: /Home/Privacy - Privacy policy (optional)
        public IActionResult Privacy()
        {
            return View();
        }

        // Error handling
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View();
        }
    }
}