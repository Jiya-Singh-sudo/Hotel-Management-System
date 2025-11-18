using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagementSystem.Controllers
{
    // Restricts access to all actions in this controller to users 
    // who were signed in with the "Admin" role claim.
    [Authorize(Roles = "Admin")]
    public class CustomersController : Controller
    {
        // GET: /Customers/Dashboard
        // This is the action method that the AccountController redirects an Admin user to.
        public IActionResult Dashboard()
        {
            // This renders the view located at Views/Customers/Dashboard.cshtml
            return View();
        }
    }
}