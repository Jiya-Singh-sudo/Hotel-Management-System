using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HotelManagementSystem.Models;
using HotelManagementSystem.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using System.Threading.Tasks;

namespace HotelManagementSystem.Controllers
{
    public class BookingsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BookingsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Bookings
        public async Task<IActionResult> Index()
        {
            var bookings = _context.Bookings
                .Include(b => b.Customer)
                .Include(b => b.Room)
                .Include(b => b.Driver);
            return View(await bookings.ToListAsync());
        }

        // GET: Bookings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var booking = await _context.Bookings
                .Include(b => b.Customer)
                .Include(b => b.Room)
                .Include(b => b.Driver)
                .FirstOrDefaultAsync(m => m.BookingId == id);

            if (booking == null) return NotFound();

            return View(booking);
        }

        // GET: Bookings/Create
        public IActionResult Create()
        {
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "Name");
            ViewData["RoomId"] = new SelectList(_context.Rooms.Where(r => r.IsAvailable), "RoomId", "RoomType");
            ViewData["DriverId"] = new SelectList(_context.Drivers.Where(d => d.IsAvailable), "DriverId", "Name");
            return View();
        }

        // POST: Bookings/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BookingId,CustomerId,RoomId,DriverId,CheckInDate,CheckOutDate,NumberOfGuests,TotalAmount")] Booking booking)
        {
            if (ModelState.IsValid)
            {
                // Mark selected room & driver unavailable
                var room = await _context.Rooms.FindAsync(booking.RoomId);
                var driver = await _context.Drivers.FindAsync(booking.DriverId);

                if (room != null) room.IsAvailable = false;
                if (driver != null) driver.IsAvailable = false;

                _context.Add(booking);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "Name", booking.CustomerId);
            ViewData["RoomId"] = new SelectList(_context.Rooms, "RoomId", "RoomType", booking.RoomId);
            ViewData["DriverId"] = new SelectList(_context.Drivers, "DriverId", "Name", booking.DriverId);
            return View(booking);
        }

        // GET: Bookings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null) return NotFound();

            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "Name", booking.CustomerId);
            ViewData["RoomId"] = new SelectList(_context.Rooms, "RoomId", "RoomType", booking.RoomId);
            ViewData["DriverId"] = new SelectList(_context.Drivers, "DriverId", "Name", booking.DriverId);
            return View(booking);
        }

        // POST: Bookings/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BookingId,CustomerId,RoomId,DriverId,CheckInDate,CheckOutDate,NumberOfGuests,TotalAmount")] Booking booking)
        {
            if (id != booking.BookingId) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(booking);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Bookings.Any(e => e.BookingId == id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "Name", booking.CustomerId);
            ViewData["RoomId"] = new SelectList(_context.Rooms, "RoomId", "RoomType", booking.RoomId);
            ViewData["DriverId"] = new SelectList(_context.Drivers, "DriverId", "Name", booking.DriverId);
            return View(booking);
        }

        // GET: Bookings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var booking = await _context.Bookings
                .Include(b => b.Customer)
                .Include(b => b.Room)
                .Include(b => b.Driver)
                .FirstOrDefaultAsync(m => m.BookingId == id);

            if (booking == null) return NotFound();

            return View(booking);
        }

        // POST: Bookings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking != null)
            {
                var room = await _context.Rooms.FindAsync(booking.RoomId);
                var driver = await _context.Drivers.FindAsync(booking.DriverId);

                if (room != null) room.IsAvailable = true;
                if (driver != null) driver.IsAvailable = true;

                _context.Bookings.Remove(booking);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
