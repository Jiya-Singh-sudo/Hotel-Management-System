// In Models/LoginViewModel.cs (Create this new file)

using System.ComponentModel.DataAnnotations;

namespace HotelManagementSystem.Models
{
    public class LoginViewModel
    {
         [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; } = string.Empty;
    }
}