using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Common;

namespace Domain.Entities
{
    public class Seat : BaseEntity
    {
        public int Id { get; set; }
        
        [Required]
        [ForeignKey("SeatTypeId")]
         public int SeatTypeId { get; set; }
        public SeatType SeatType { get; set; }

        [Required]
        [ForeignKey("CompartmentId")]
        public int CompartmentId { get; set; }
        public Compartment Compartment { get; set; }

        [StringLength(100)] public string Status { get; set; }

    }
}