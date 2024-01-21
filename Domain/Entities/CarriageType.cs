using System.ComponentModel.DataAnnotations;
using Domain.Common;

namespace Domain.Entities
{
    public class CarriageType : BaseEntity
    {
        public int Id { get; set; }

        [Required][StringLength(100)] public string Name { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Please enter a number of compartments bigger than 0")]
        public int NumberOfCompartments { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Please enter a number of seats bigger than 0")]
        public int NumberOfSeats { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Please enter a number of seat types bigger than 0")]
        public int NumberOfSeatTypes { get; set; }

        public double ServiceCharge { get; set; }

        [Required][StringLength(450)] public string Description { get; set; }

        public string Status { get; set; }
    }
}