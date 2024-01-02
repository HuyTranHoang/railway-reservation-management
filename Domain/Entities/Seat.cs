using System.ComponentModel.DataAnnotations;
using Domain.Common;

namespace Domain.Entities
{
    public class Seat : BaseEntity
    {
        public int Id { get; set; }
        
        [Required] public int SeatTypeId { get; set; }
        public SeatType SeatType { get; set; }

        [Required] public int CompartmentId { get; set; }
        public Compartment Compartment { get; set; }

        [StringLength(100)] public string Status { get; set; }

    }
}