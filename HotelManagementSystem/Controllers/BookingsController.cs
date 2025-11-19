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

        // GET: /Bookings/Index - Show all bookings
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

        // GET: /Bookings/Create - Show booking form with dropdown data
        [HttpGet]
        [Route("Create")]
        public async Task<IActionResult> Create()
        {
            Console.WriteLine("=== CREATE BOOKING PAGE CALLED ===");

            await PopulateDropdowns();

            return View();
        }

        // POST: /Bookings/Create - Handle booking submission
        [HttpPost]
        [Route("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Booking model)
        {
            Console.WriteLine("=== CREATE BOOKING SUBMISSION ===");
            Console.WriteLine($"Customer ID: {model.CustomerId}, Room ID: {model.RoomId}");

            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine($"Validation Error: {error.ErrorMessage}");
                }

                // Repopulate dropdowns
                await PopulateDropdowns();
                ViewBag.Error = "Please fill all required fields correctly.";
                return View(model);
            }

            // Validate dates
            if (model.CheckOutDate <= model.CheckInDate)
            {
                await PopulateDropdowns();
                ViewBag.Error = "Check-out date must be after check-in date.";
                return View(model);
            }

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
                Console.WriteLine($"✗ Error: {ex.Message}");
                Console.WriteLine($"✗ Stack Trace: {ex.StackTrace}");
                await PopulateDropdowns();
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

        // Helper method to populate dropdowns
        private async Task PopulateDropdowns()
        {
            Console.WriteLine("=== POPULATING DROPDOWNS ===");

            // ✅ FIX 1: Fetch customers from database
            var customers = await _context.Customers.ToListAsync();
            Console.WriteLine($"Found {customers.Count} customers");
            
            ViewBag.CustomerId = customers.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = $"{c.Name} ({c.Email})"
            }).ToList();

            // ✅ FIX 2: Fetch rooms from database
            var rooms = await _context.Rooms.ToListAsync();
            Console.WriteLine($"Found {rooms.Count} rooms");
            
            ViewBag.RoomId = rooms.Select(r => new SelectListItem
            {
                Value = r.Id.ToString(),
                Text = $"{r.RoomType} - ${r.PricePerNight}/night (Room {r.RoomNumber})"
            }).ToList();

            // ✅ FIX 3: Fetch drivers from database
            var drivers = await _context.Drivers.ToListAsync();
            Console.WriteLine($"Found {drivers.Count} drivers");
            
            var driverList = new List<SelectListItem>
            {
                new SelectListItem { Value = "", Text = "-- No Driver Required --" }
            };
            
            driverList.AddRange(drivers.Select(d => new SelectListItem
            {
                Value = d.Id.ToString(),
                Text = $"{d.Name} - {d.Phone} {(d.IsAvailable ? "(Available)" : "(Unavailable)")}"
            }));
            
            ViewBag.DriverId = driverList;

            // Debug output
            Console.WriteLine("Dropdowns populated successfully");
        }
    }
}