using Microsoft.AspNetCore.Mvc;
using HotelManagementSystem.Models;

namespace HotelManagementSystem.Controllers
{
    [Route("Contact")]  // Add this
    public class ContactController : Controller
    {
        [HttpGet]
        [Route("")]  // Matches /Contact
        [Route("Index")]  // Matches /Contact/Index
        public IActionResult Index()
        {
            Console.WriteLine("=== INDEX METHOD CALLED ===");
            return View();
        }

        [HttpPost]
        [Route("Send")]  // Explicitly matches /Contact/Send
        public IActionResult Send(string Name, string Email, string Message)
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

            ViewBag.Message = "Thank you! Your message has been received.";
            return View("Index");
        }

        [HttpGet]
        [Route("Test")]
        public IActionResult Test()
        {
            Console.WriteLine("=== TEST METHOD CALLED ===");
            return Content("Contact controller is working!");
        }
    }
}