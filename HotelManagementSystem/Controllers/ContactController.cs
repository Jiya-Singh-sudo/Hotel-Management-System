using Microsoft.AspNetCore.Mvc;
using HotelManagementSystem.Models;
using HotelManagementSystem.Data;

namespace HotelManagementSystem.Controllers
{
    [Route("Contact")]
    public class ContactController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ContactController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("")]
        [Route("Index")]
        public IActionResult Index()
        {
            Console.WriteLine("=== INDEX METHOD CALLED ===");
            return View();
        }

        [HttpPost]
        [Route("Send")]
        public async Task<IActionResult> Send(string Name, string Email, string Message)
        {
            Console.WriteLine("=== SEND METHOD CALLED ===");
            Console.WriteLine($"Name: {Name}");
            Console.WriteLine($"Email: {Email}");
            Console.WriteLine($"Message: {Message}");

            if (string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Message))
            {
                ViewBag.Error = "All fields are required.";
                return View("Index");
            }

            try
            {
                // Save to database
                var contact = new Contact
                {
                    Name = Name,
                    Email = Email,
                    Message = Message,
                    CreatedAt = DateTime.UtcNow
                };

                _context.Contacts.Add(contact);
                await _context.SaveChangesAsync();

                Console.WriteLine($"✓ Contact saved to database with ID: {contact.Id}");
                
                ViewBag.Message = "Thank you! Your message has been received.";
                return View("Index");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"✗ Error saving to database: {ex.Message}");
                ViewBag.Error = "Failed to save your message. Please try again.";
                return View("Index");
            }
        }
    }
}