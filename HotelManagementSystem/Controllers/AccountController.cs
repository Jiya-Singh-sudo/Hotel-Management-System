using Microsoft.AspNetCore.Mvc;
using HotelManagementSystem.Data; 
using HotelManagementSystem.Models.ViewModels;
using System.Threading.Tasks;

namespace HotelManagementSystem.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;

        // You would typically inject a SignInManager/UserManager here for real authentication
        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Account/Login - Displays the Login page
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // POST: /Account/Login - Handles form submission
        [HttpPost]
        [ValidateAntiForgeryToken]
        // Uses the new LoginViewModel
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                // In a real application, you would check credentials here.
                // Placeholder for authentication logic:
                if (model.Email == "admin@staywise.com" && model.Password == "password") 
                {
                    // Successful login: Redirect to the Customers Dashboard 
                    return RedirectToAction("Dashboard", "Customers"); 
                }
                
                // Failed login
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                ViewBag.Error = "Invalid email or password. Please try again.";
            }

            // If ModelState is invalid or login failed, return to the view
            return View(model);
        }

        // GET: /Account/Register - Displays the Register page
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        
        // POST: /Account/Register - Handles form submission
        [HttpPost]
        [ValidateAntiForgeryToken]
        // Uses the new RegisterViewModel
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Placeholder for user creation logic (e.g., saving to _context.Customers)
                // In a real application, you would hash the password and save the new user.
                
                // For simplicity, we just redirect to the login page
                return RedirectToAction(nameof(Register));
            }
            
            // If ModelState is invalid, return to the registration view to show errors
            return View(model);
        }

        // GET: /Account/Logout
        public IActionResult Logout()
        {
            // Logic to sign the user out goes here
            return RedirectToAction(nameof(Login));
        }
    }
}