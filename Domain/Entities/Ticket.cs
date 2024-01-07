using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Common;

namespace Domain.Entities
{
    public class Ticket : BaseEntity
    {
        public int Id { get; set; }

        [Required]
        [ForeignKey("PassengerId")]
        public int PassengerId { get; set; }
        public Passenger Passenger { get; set; }
        
        [Required]
        [ForeignKey("TrainId")]
        public int TrainId { get; set; }
        public Train Train { get; set; }

        [Required]
        [ForeignKey("DistanceFareId")]
        public int DistanceFareId { get; set; }
        public DistanceFare DistanceFare { get; set; }

        [Required]
        [ForeignKey("CarriageId")]
        public int CarriageId { get; set; }
        public Carriage Carriage { get; set; }

        [Required]
        [ForeignKey("SeatId")]
        public int SeatId { get; set; }
        public Seat Seat { get; set; }

        [Required]
        [ForeignKey("ScheduleId")]
        public int ScheduleId { get; set; }
        public Schedule Schedule { get; set; }

        [Required]
        [ForeignKey("PaymentId")]
        public int PaymentId { get; set; }
        public Payment Payment { get; set; }

        [StringLength(100)] public string Status { get; set; }
    }
}