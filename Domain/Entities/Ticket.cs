using System.ComponentModel.DataAnnotations;
using Domain.Common;

namespace Domain.Entities
{
    public class Ticket : BaseEntity
    {
        public int Id { get; set; }

        public int PassengerId { get; set; }
        public Passenger Passenger { get; set; }
        
        [Required] public int TrainId { get; set; }
        public Train Train { get; set; }

        [Required] public int DistanceFareId { get; set; }
        public DistanceFare DistanceFare { get; set; }

        [Required] public int CarriageId { get; set; }
        public Carriage Carriage { get; set; }

        [Required] public int SeatId { get; set; }
        public Seat Seat { get; set; }

        [Required] public int ScheduleId { get; set; }
        public Schedule Schedule { get; set; }

        [StringLength(100)] public string Status { get; set; }
    }
}