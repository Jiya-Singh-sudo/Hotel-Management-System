using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelManagementSystem.Models
{
    public class Booking
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime CheckInDate { get; set; }

        [Required]
        public DateTime CheckOutDate { get; set; }

        [Required]
        public int NumberOfGuests { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }

        // Foreign Keys
        [Required]
        public int CustomerId { get; set; }
        
        [Required]
        public int RoomId { get; set; }
        
        public int? DriverId { get; set; }

        // Navigation Properties
        [ForeignKey("CustomerId")]
        public Customer? Customer { get; set; }

        [ForeignKey("RoomId")]
        public Room? Room { get; set; }

        [ForeignKey("DriverId")]
        public Driver? Driver { get; set; }
    }
}