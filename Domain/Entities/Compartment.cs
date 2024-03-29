using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Common;

namespace Domain.Entities
{
    public class Compartment : BaseEntity
    {
        public int Id { get; set; }

        [Required] [StringLength(100)] public string Name { get; set; }

        [Required]
        [ForeignKey("CarriageId")]
        public int CarriageId { get; set; }
        public Carriage Carriage { get; set; }

        [StringLength(100)] public string Status { get; set; }

        public ICollection<Seat> Seats { get; set; }
    }
}