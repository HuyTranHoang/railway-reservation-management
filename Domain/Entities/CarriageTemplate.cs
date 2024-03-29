using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Common;

namespace Domain.Entities
{
    public class CarriageTemplate : BaseEntity
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [ForeignKey("CarriageTypeId")]
        public int CarriageTypeId { get; set; }
        public CarriageType CarriageType { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Please enter a number of compartments bigger than 0")]
        public int NumberOfCompartments { get; set; }

        [StringLength(100)]
        public string Status { get; set; }
    }
}