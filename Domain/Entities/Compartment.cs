using System.ComponentModel.DataAnnotations;
using Domain.Common;

namespace Domain.Entities
{
    public class Compartment : BaseEntity
    {
        public int Id { get; set; }

        [Required] [StringLength(100)] public string Name { get; set; }

        public int CarriageId { get; set; }
        public Carriage Carriage { get; set; }

        [Required] public int NumberOfSeats { get; set; }

        [StringLength(100)] public string Status { get; set; }
    }
}