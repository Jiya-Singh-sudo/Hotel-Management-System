using System.ComponentModel.DataAnnotations;

namespace HotelManagementSystem.Models
{
    public class BookingCreateViewModel
    {
        [Required]
        [Display(Name = "Customer Name")]
        public string CustomerName { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Room ID")]
        public int RoomId { get; set; }

        [Display(Name = "Driver ID (Optional)")]
        public int? DriverId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Check-In Date")]
        public DateTime CheckInDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Check-Out Date")]
        public DateTime CheckOutDate { get; set; }

        [Required]
        [Range(1, 20)]
        [Display(Name = "Number of Guests")]
        public int NumberOfGuests { get; set; }

        [Required]
        [Display(Name = "Total Amount")]
        public decimal TotalAmount { get; set; }
    }
}