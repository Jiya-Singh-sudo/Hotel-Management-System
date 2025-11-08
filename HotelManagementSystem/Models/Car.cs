using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelManagementSystem.Models
{
    public class Car
    {
        [Key]
        public int CarId { get; set; }

        [Required]
        [StringLength(50)]
        public string Make { get; set; } = string.Empty; // e.g., Toyota, BMW

        [Required]
        [StringLength(50)]
        public string Model { get; set; } = string.Empty; // e.g., Innova, X5

        [Required]
        [StringLength(15)]
        public string RegistrationNumber { get; set; } = string.Empty;

        [Required]
        public int DriverId { get; set; }

        [ForeignKey("DriverId")]
        public Driver Driver { get; set; } = null!;
    }
}
