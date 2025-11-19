using HotelManagementSystem.Data;
using HotelManagementSystem.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using BCrypt.Net;

namespace HotelManagementSystem.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Account/Register - Loads the registration form
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // POST: /Account/Register - Handles form submission
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel? model)
        {
            // Check if model is null
            if (model == null)
            {
                ViewBag.Error = "Invalid registration data.";
                return View();
            }

            // Log received data for debugging
            Console.WriteLine($"Registration attempt - Name: {model.Name}, Email: {model.Email}");

            // FIX: Check if model state is INVALID (not valid!)
            if (!ModelState.IsValid)
            {
                // Log validation errors
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine($"Validation Error: {error.ErrorMessage}");
                }

                ViewBag.Error = "Registration failed. Please correct the errors below.";
                return View(model);
            }

            try
            {
                // Check if user already exists based on Email
                if (await _context.Customers.AnyAsync(c => c.Email == model.Email))
                {
                    ModelState.AddModelError("Email", "A user with this email already exists.");
                    ViewBag.Error = "Registration failed: A user with this email already exists.";
                    return View(model);
                }

                // Hash the password securely
                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(model.Password);

                // Create new customer
                var newUser = new Customer
                {
                    Name = model.Name,
                    Email = model.Email,
                    Username = model.Email, // Use Email as Username
                    Phone = model.PhoneNumber,
                    PasswordHash = hashedPassword, // Store hashed password
                    IsAdmin = false, // Default new user to Customer role
                    Bookings = new List<Booking>()
                };

                _context.Customers.Add(newUser);
                await _context.SaveChangesAsync();

                Console.WriteLine($"User registered successfully: {model.Email}");
                ViewBag.Message = "Account created successfully! Please login.";
                return RedirectToAction("Login");
            }
            catch (DbUpdateException dbEx)
            {
                Console.WriteLine($"Database Error: {dbEx.InnerException?.Message ?? dbEx.Message}");
                ViewBag.Error = "Registration failed due to database error. Please try again.";
                return View(model);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected Error: {ex.Message}");
                ViewBag.Error = $"Registration failed: {ex.Message}";
                return View(model);
            }
        }

        // GET: /Account/Login
        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel? model, string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            // Check if model is null
            if (model == null)
            {
                ViewBag.Error = "Invalid login data.";
                return View();
            }

            // Log login attempt
            Console.WriteLine($"Login attempt - Username: {model.Email}");

            // FIX: Check if model state is INVALID (not valid!)
            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine($"Validation Error: {error.ErrorMessage}");
                }
                return View(model);
            }

            try
            {
                // Find the user by username (using Email as Username)
                var user = await _context.Customers
                    .FirstOrDefaultAsync(c => c.Username == model.Email);

                // Verify password using BCrypt
                bool passwordVerified = (user != null && BCrypt.Net.BCrypt.Verify(model.Password, user.PasswordHash));

                if (passwordVerified)
                {
                    Console.WriteLine($"Login successful for user: {user!.Username}");

                    // Create Claims Identity
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                        new Claim(ClaimTypes.Name, user.Username ?? string.Empty),
                        new Claim(ClaimTypes.Email, user.Email ?? string.Empty),
                        new Claim(ClaimTypes.Role, user.IsAdmin ? "Admin" : "Customer")
                    };

                    var claimsIdentity = new ClaimsIdentity(
                        claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    var authProperties = new AuthenticationProperties
                    {
                        IsPersistent = true, // Keep user logged in after browser closes
                        ExpiresUtc = DateTimeOffset.UtcNow.AddHours(24) // Session expires in 24 hours
                    };

                    // Sign the user in (creates the authentication cookie)
                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity),
                        authProperties);

                    // Redirect based on role
                    if (user.IsAdmin)
                    {
                        Console.WriteLine($"Redirecting admin {user.Username} to Dashboard");
                        return RedirectToAction("Dashboard", "Customers");
                    }
                    else
                    {
                        Console.WriteLine($"Redirecting user {user.Username} to Home page");
                        return RedirectToAction("Index", "Home");
                    }
                }

                // Invalid credentials
                Console.WriteLine($"Login failed for username: {model.Email}");
                ModelState.AddModelError(string.Empty, "Invalid username or password.");
                ViewBag.Error = "Invalid username or password.";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Login Error: {ex.Message}");
                ModelState.AddModelError(string.Empty, "An error occurred during login. Please try again.");
                ViewBag.Error = "An error occurred during login. Please try again.";
            }

            return View(model);
        }

        // POST: /Account/Logout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            try
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                Console.WriteLine("User logged out successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Logout Error: {ex.Message}");
            }

            return RedirectToAction("Index", "Home");
        }

        // Optional: GET method for Logout (for direct link access)
        [HttpGet]
        public async Task<IActionResult> LogoutConfirm()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
    }
}