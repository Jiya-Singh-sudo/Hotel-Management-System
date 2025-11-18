// In Models/LoginViewModel.cs (Create this new file)

using System.ComponentModel.DataAnnotations;

namespace HotelManagementSystem.Models
{
    public class LoginViewModel
    {
        [Required]
        public string? Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
    }
}