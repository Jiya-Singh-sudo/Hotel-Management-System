using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HotelManagementSystem.Data;
using HotelManagementSystem.Models;
using System.Security.Claims;


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

        // GET: /Bookings/Create - Show booking form with dropdown data
        [HttpGet]
        [Route("Create")]
        public async Task<IActionResult> Create()
        {
            Console.WriteLine("=== CREATE BOOKING PAGE CALLED ===");

            try
            {
                // Populate Customers dropdown
                var customers = await _context.Customers
                    .Select(c => new SelectListItem
                    {
                        Value = c.Id.ToString(),
                        Text = c.Name + " (" + c.Email + ")"
                    })
                    .ToListAsync();
                
                Console.WriteLine($"Found {customers.Count} customers");
                
                // If no customers, create sample data
                if (!customers.Any())
                {
                    await SeedSampleCustomers();
                    customers = await _context.Customers
                        .Select(c => new SelectListItem
                        {
                            Value = c.Id.ToString(),
                            Text = c.Name + " (" + c.Email + ")"
                        })
                        .ToListAsync();
                }
                
                ViewBag.CustomerId = customers;

                // Populate Rooms dropdown (create sample rooms if none exist)
                var rooms = await _context.Rooms.ToListAsync();
                Console.WriteLine($"Found {rooms.Count} rooms");
                
                if (!rooms.Any())
                {
                    // Add sample rooms if database is empty
                    await SeedSampleRooms();
                    rooms = await _context.Rooms.ToListAsync();
                }

                ViewBag.RoomId = rooms.Select(r => new SelectListItem
                {
                    Value = r.Id.ToString(),
                    Text = r.RoomType + " - $" + r.PricePerNight + "/night (Room " + r.RoomNumber + ")"
                }).ToList();

                // Populate Drivers dropdown
                var drivers = await _context.Drivers.ToListAsync();
                Console.WriteLine($"Found {drivers.Count} drivers");
                
                if (!drivers.Any())
                {
                    await SeedSampleDrivers();
                    drivers = await _context.Drivers.ToListAsync();
                }

                var driverList = new List<SelectListItem>
                {
                    new SelectListItem { Value = "", Text = "-- No Driver Required --" }
                };
                
                driverList.AddRange(drivers.Select(d => new SelectListItem
                {
                    Value = d.Id.ToString(),
                    Text = d.Name
                }));

                ViewBag.DriverId = driverList;

                Console.WriteLine("=== DROPDOWNS POPULATED SUCCESSFULLY ===");
                return View();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR in Create GET: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");
                ViewBag.Error = "Failed to load booking form: " + ex.Message;
                return View();
            }
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
                await PopulateDropdowns();
                ViewBag.Error = "Failed to create booking. Please try again.";
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
            var customers = await _context.Customers
                .Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name + " (" + c.Email + ")"
                })
                .ToListAsync();
            ViewBag.CustomerId = customers;

            var rooms = await _context.Rooms
                .Select(r => new SelectListItem
                {
                    Value = r.Id.ToString(),
                    Text = r.RoomType + " - $" + r.PricePerNight + "/night (Room " + r.RoomNumber + ")"
                })
                .ToListAsync();
            ViewBag.RoomId = rooms;

            var drivers = await _context.Drivers.ToListAsync();
            var driverList = new List<SelectListItem>
            {
                new SelectListItem { Value = "", Text = "-- No Driver Required --" }
            };
            driverList.AddRange(drivers.Select(d => new SelectListItem
            {
                Value = d.Id.ToString(),
                Text = d.Name
            }));
            ViewBag.DriverId = driverList;
        }

        // Helper method to seed sample customers
        private async Task SeedSampleCustomers()
        {
            var sampleCustomers = new List<Customer>
            {
                new Customer
                {
                    Name = "John Doe",
                    Email = "john.doe@example.com",
                    Phone = "1234567890"
                },
                new Customer
                {
                    Name = "Jane Smith",
                    Email = "jane.smith@example.com",
                    Phone = "0987654321"
                },
                new Customer
                {
                    Name = "Mike Johnson",
                    Email = "mike.j@example.com",
                    Phone = "5555555555"
                }
            };

            _context.Customers.AddRange(sampleCustomers);
            await _context.SaveChangesAsync();
            Console.WriteLine("✓ Sample customers added to database");
        }

        // Helper method to seed sample drivers
        private async Task SeedSampleDrivers()
        {
            var sampleDrivers = new List<Driver>
            {
                new Driver
                {
                    Name = "John Driver",
                    Phone = "1112223333",
                    LicenseNumber = "DL123456"
                },
                new Driver
                {
                    Name = "Jane Driver",
                    Phone = "4445556666",
                    LicenseNumber = "DL789012"
                }
            };

            _context.Drivers.AddRange(sampleDrivers);
            await _context.SaveChangesAsync();
            Console.WriteLine("✓ Sample drivers added to database");
        }

        // Helper method to seed sample rooms
        private async Task SeedSampleRooms()
        {
            var sampleRooms = new List<Room>
            {
                new Room
                {
                    RoomNumber = "101",
                    RoomType = "Standard",
                    PricePerNight = 100.00m,
                    Status = "Available"
                },
                new Room
                {
                    RoomNumber = "201",
                    RoomType = "Deluxe",
                    PricePerNight = 150.00m,
                    Status = "Available"
                },
                new Room
                {
                    RoomNumber = "301",
                    RoomType = "Suite",
                    PricePerNight = 250.00m,
                    Status = "Available"
                }
            };

            _context.Rooms.AddRange(sampleRooms);
            await _context.SaveChangesAsync();
            Console.WriteLine("✓ Sample rooms added to database");
        }
    }
}