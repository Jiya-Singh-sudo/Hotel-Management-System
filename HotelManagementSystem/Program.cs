using Microsoft.EntityFrameworkCore;
using HotelManagementSystem.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using BCrypt.Net; 
using HotelManagementSystem.Models; // <--- THIS WAS MISSING

var builder = WebApplication.CreateBuilder(args);
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews();

// Configure PostgreSQL connection
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.LogoutPath = "/Account/Logout";
    });

var app = builder.Build();

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.UseStaticFiles();

// Standard Razor Pages routing
app.MapRazorPages(); 

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// ==============================================================
// AUTO-SEED DEFAULT ADMIN USER
// ==============================================================
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();
        
        // Ensure the database exists
        context.Database.EnsureCreated();

        var adminEmail = "admin@staywise.com";
        
        // Check if the admin already exists
        if (!context.Customers.Any(u => u.Email == adminEmail))
        {
            var adminUser = new Customer
            {
                Name = "System Admin",
                Email = adminEmail,
                Username = adminEmail, 
                Phone = "0000000000",
                // Ensure you have the BCrypt.Net-Next package installed for this to work
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123"), 
                IsAdmin = true 
            };

            context.Customers.Add(adminUser);
            context.SaveChanges();
            Console.WriteLine("Admin User Created Successfully!");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error creating admin user: {ex.Message}");
    }
}
// ==============================================================

app.Run();