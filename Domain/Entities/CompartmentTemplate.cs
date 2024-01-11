using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class CompartmentTemplate
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Please enter a value bigger than 0")]
        public int NumberOfSeats { get; set; }

        [StringLength(100)]
        public string Status { get; set; }
    }
}