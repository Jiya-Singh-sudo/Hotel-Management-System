using HotelManagementSystem.Data;
using HotelManagementSystem.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace HotelManagementSystem.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Account/Login
        public IActionResult Login(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View(); // Assumes you have a Login.cshtml view under Views/Account
        }

        // POST: /Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (ModelState.IsValid)
            {
                // 1. Find the user by username
                var user = await _context.Customers
                    .FirstOrDefaultAsync(c => c.Username == model.Username);

                // 2. Validate Credentials
                // WARNING: THIS IS INSECURE. In a real application, you MUST use a 
                // robust password hashing library (e.g., Argon2 or built-in Identity methods).
                if (user != null && user.PasswordHash == model.Password)
                {
                    // 3. Create Claims Identity
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                        // Username may be nullable; ensure a non-null value is provided to Claim
                        new Claim(ClaimTypes.Name, user.Username ?? string.Empty),
                        // Assign a Role claim based on IsAdmin property
                        new Claim(ClaimTypes.Role, user.IsAdmin ? "Admin" : "Customer")
                    };

                    var claimsIdentity = new ClaimsIdentity(
                        claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    var authProperties = new AuthenticationProperties
                    {
                        // Allow the user to remain logged in after the browser is closed (optional)
                        IsPersistent = true 
                    };

                    // 4. Sign the user in (creates the authentication cookie)
                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity),
                        authProperties);

                    // 5. CONDITIONAL REDIRECTION (Fulfills user request)
                   if (user.IsAdmin)
                    {
                        // Redirect Admin to CustomersController's Dashboard action
                        return RedirectToAction("Dashboard", "Customers"); 
                    }
                    else if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    {
                        // Standard user redirect to requested URL (if they tried to access a protected page)
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        // Standard user default redirect:
                        // This redirects to the application root, which typically serves the 
                        // Razor Page at Pages/Index.cshtml due to the default routing setup.
                        return RedirectToAction("Index", "Home"); 
                    }
                }
                
                ModelState.AddModelError(string.Empty, "Invalid username or password.");
            }
            
            // If model state is invalid or login failed
            return View(model);
        }

        // POST: /Account/Logout
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
    }
}