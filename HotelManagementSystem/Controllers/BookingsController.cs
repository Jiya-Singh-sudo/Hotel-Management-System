using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HotelManagementSystem.Data;
using HotelManagementSystem.Models;
using System.Threading.Tasks;
using System.Linq;

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

        // GET: /Bookings/Index - Show all bookings (with Eager Loading)
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

        // GET: /Bookings/Create - Show booking form (Passes the ViewModel)
        [HttpGet]
        [Route("Create")]
        public IActionResult Create()
        {
            Console.WriteLine("=== CREATE BOOKING PAGE CALLED ===");
            
            // FIX for CS9035: Initialize the ViewModel's required string properties
            return View(new BookingCreateViewModel 
            {
                CustomerName = string.Empty
            }); 
        }

        // POST: /Bookings/Create - Handle booking submission
        [HttpPost]
        [Route("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BookingCreateViewModel model) 
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
            
            // 2. Validate Customer Name existence and retrieve ID
            var customer = await _context.Customers
                .FirstOrDefaultAsync(c => c.Name == model.CustomerName);
                
            if (customer == null)
            {
                ModelState.AddModelError("CustomerName", "Customer name not found. Please ensure the name is registered.");
                ViewBag.Error = "The provided Customer Name is invalid.";
                return View(model);
            }
            
            // 3. Validate Room ID existence and availability
            var room = await _context.Rooms.FirstOrDefaultAsync(r => r.Id == model.RoomId);
            if (room == null)
            {
                ModelState.AddModelError("RoomId", "Room ID not found.");
                ViewBag.Error = "The provided Room ID is invalid.";
                return View(model);
            }
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
                // FIX: Construct the final Booking entity using the found Customer ID
                var bookingEntity = new Booking
                {
                    CustomerId = customer.Id, // Assign the found Customer ID (resolves CS0117)
                    RoomId = model.RoomId,
                    DriverId = model.DriverId,
                    CheckInDate = model.CheckInDate,
                    CheckOutDate = model.CheckOutDate,
                    NumberOfGuests = model.NumberOfGuests,
                    TotalAmount = model.TotalAmount,
                    Status = "Pending",
                    CreatedAt = DateTime.UtcNow
                };
                
                _context.Bookings.Add(bookingEntity);
                await _context.SaveChangesAsync();

                Console.WriteLine($"✓ Booking saved with ID: {bookingEntity.Id}");

                return RedirectToAction("Success", new { id = bookingEntity.Id });
            }
            catch (Exception ex)
            {
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
    }
}