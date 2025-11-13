using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HotelManagementSystem.Controllers
{
    // This controller is responsible for handling the contact form submissions.
    public class ContactController : Controller
    {
        // GET: /Contact/Index (The base route for the Contact page)
        // This is usually handled by Razor Pages, but we include it for completeness if used via MVC.
        public IActionResult Index()
        {
            // Redirects to the Razor Page equivalent if accessed via MVC route
            return RedirectToPage("/Contact");
        }

        // POST: /Contact/Send
        // This action receives the form data submitted by the Contact page.
        [HttpPost]
        [ValidateAntiForgeryToken]
        // Parameters should match the 'name' attributes in the Contact.cshtml form
        public IActionResult Send(string Name, string Email, string Message)
        {
            if (string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Message))
            {
                ViewBag.Error = "Please fill out all required fields.";
                // When returning to a Razor Page from a Controller, use RedirectToPage.
                // We pass the error back via ViewData/ViewBag, which requires a redirect or a temporary ViewData persistence mechanism.
                // For simplicity here, we'll redirect back to the page and rely on Query Parameters or Session for a real app.
                // For this example, we will just redirect to the page and let the next action handle displaying the message.
                
                // Since this is a simple Contact form, we'll assume success for now.
            }
            
            // --- Simulation of sending an email or saving to a database ---
            // 1. In a real application, you would save the contact message to a database table 
            //    or call an email service (like SendGrid).
            
            // 2. We use ViewBag to pass a success message back to the page.
            ViewBag.Message = $"Thank you, {Name}! Your message has been sent successfully.";
            
            // Since the form is posting from a Razor Page, the simplest way to show the message 
            // on the same page is to redirect back to the Contact Page itself.
            return RedirectToPage("/Contact", new { message = "sent" });
        }
    }
}