using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Common;

namespace Domain.Entities
{
    public class Schedule : BaseEntity
    {
        public int Id { get; set; }

        [StringLength(100)] public string Name { get; set; }
  
        [Required]
        [ForeignKey("TrainId")]
        public int TrainId { get; set; }
        public Train Train { get; set; }

        [Required]
        [ForeignKey("DepartureStationId")]
        public int DepartureStationId { get; set; }
        public TrainStation DepartureStation { get; set; }

        [Required]
        [ForeignKey("ArrivalStationId")]
        public int ArrivalStationId { get; set; }
        public TrainStation ArrivalStation { get; set; }

        [Required] public DateTime DepartureTime { get; set; }

        [Required] public int Duration { get; set; }

        [StringLength(100)] public string Status { get; set; }

    }
}