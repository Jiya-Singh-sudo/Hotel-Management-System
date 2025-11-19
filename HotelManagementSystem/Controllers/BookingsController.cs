using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HotelManagementSystem.Data;
using HotelManagementSystem.Models;

namespace HotelManagementSystem.Controllers
{
    [Route("Bookings")]
    public class BookingsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BookingsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Bookings/Index - Show all bookings (Include statements are correctly here)
        [HttpGet]
        [Route("")]
        [Route("Index")]
        public async Task<IActionResult> Index()
        {
            var bookings = await _context.Bookings
                .Include(b => b.Customer)
                .Include(b => b.Room)
                .Include(b => b.Driver)
                .ToListAsync();

            return View(bookings);
        }

        // GET: /Bookings/Create - Show booking form
        [HttpGet]
        [Route("Create")]
        public IActionResult Create()
        {
            Console.WriteLine("=== CREATE BOOKING PAGE CALLED ===");
            return View();
        }

        // POST: /Bookings/Create - Handle booking submission
        [HttpPost]
        [Route("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Booking model)
        {
            Console.WriteLine("=== CREATE BOOKING SUBMISSION ===");

            if (!ModelState.IsValid)
            {
                ViewBag.Error = "Please fill all required fields correctly.";
                return View(model);
            }

            // --- CRITICAL VALIDATION LOGIC ---

            // 1. Validate Check-out Date
            if (model.CheckOutDate <= model.CheckInDate)
            {
                ViewBag.Error = "Check-out date must be after check-in date.";
                return View(model);
            }
            
            // 2. Validate Customer ID existence
            var customerExists = await _context.Customers.AnyAsync(c => c.Id == model.CustomerName);
            if (!customerExists)
            {
                ModelState.AddModelError("CustomerId", "Customer ID not found.");
                ViewBag.Error = "The provided Customer ID is invalid.";
                return View(model);
            }

            // 3. Validate Room ID existence and availability (optional extra check)
            var room = await _context.Rooms.FirstOrDefaultAsync(r => r.Id == model.RoomId);
            if (room == null)
            {
                ModelState.AddModelError("RoomId", "Room ID not found.");
                ViewBag.Error = "The provided Room ID is invalid.";
                return View(model);
            }
            // Optional Business Logic Check:
            if (room.Status != "Available") 
            {
                ModelState.AddModelError("RoomId", $"Room {room.RoomNumber} is currently {room.Status}.");
                ViewBag.Error = $"Room {room.RoomNumber} is currently unavailable.";
                return View(model);
            }

            // 4. Validate Driver ID existence (only if provided)
            if (model.DriverId.HasValue)
            {
                var driverExists = await _context.Drivers.AnyAsync(d => d.Id == model.DriverId.Value);
                if (!driverExists)
                {
                    ModelState.AddModelError("DriverId", "Driver ID not found.");
                    ViewBag.Error = "The provided Driver ID is invalid.";
                    return View(model);
                }
            }
            
            // --- END VALIDATION LOGIC ---

            try
            {
                // Set default values
                model.Status = "Pending";
                model.CreatedAt = DateTime.UtcNow;

                _context.Bookings.Add(model);
                await _context.SaveChangesAsync();

                Console.WriteLine($"✓ Booking saved with ID: {model.Id}");

                return RedirectToAction("Success", new { id = model.Id });
            }
            catch (Exception ex)
            {
                // Note: Generic error handling is still the fallback for unexpected issues
                Console.WriteLine($"✗ Error: {ex.Message}");
                ViewBag.Error = $"Failed to create booking: {ex.Message}";
                return View(model);
            }
        }

        // GET: /Bookings/Success/5
        [HttpGet]
        [Route("Success/{id}")]
        public async Task<IActionResult> Success(int id)
        {
            var booking = await _context.Bookings
                .Include(b => b.Customer)
                .Include(b => b.Room)
                .Include(b => b.Driver)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (booking == null)
            {
                return NotFound();
            }

            return View(booking);
        }
        
        // Helper method is now obsolete and removed for cleanliness
        // private async Task PopulateDropdowns() {}
    }
}