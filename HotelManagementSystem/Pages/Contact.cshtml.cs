using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HotelManagementSystem.Pages
{
    // The Contact page model handles non-database logic for this page.
    public class ContactModel : PageModel
    {
        // This property will capture the 'message' query parameter if present in the URL
        [BindProperty(SupportsGet = true)]
        public string? Message { get; set; }

        public void OnGet()
        {
            if (Message == "sent")
            {
                // Set ViewBag message which the .cshtml file will display
                ViewData["Message"] = "Thank you! Your message has been sent successfully.";
            }
        }
        
        // We will remove OnPost since the form posts to the MVC controller
    }
}