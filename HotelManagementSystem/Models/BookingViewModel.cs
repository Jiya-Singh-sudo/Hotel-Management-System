using System.ComponentModel.DataAnnotations;


namespace HotelManagementSystem.Models
{
    public class BookingViewModel
    {
        [Required(ErrorMessage = "Guest name is required")]
        public string GuestName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Phone number is required")]
        [Phone(ErrorMessage = "Invalid phone number")]
        public string Phone { get; set; } = string.Empty;

        [Required(ErrorMessage = "Check-in date is required")]
        [DataType(DataType.Date)]
        public DateTime CheckInDate { get; set; } = DateTime.Today;

        [Required(ErrorMessage = "Check-out date is required")]
        [DataType(DataType.Date)]
        public DateTime CheckOutDate { get; set; } = DateTime.Today.AddDays(1);

        [Required(ErrorMessage = "Number of guests is required")]
        [Range(1, 10, ErrorMessage = "Number of guests must be between 1 and 10")]
        public int NumberOfGuests { get; set; } = 1;

        [Required(ErrorMessage = "Room type is required")]
        public string RoomType { get; set; } = string.Empty;

        public string? SpecialRequests { get; set; }
    }
}