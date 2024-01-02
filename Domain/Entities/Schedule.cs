using System.ComponentModel.DataAnnotations;
using Domain.Common;

namespace Domain.Entities
{
    public class Schedule : BaseEntity
    {
        public int Id { get; set; }
        
        public int TrainId { get; set; }
        public Train Train { get; set; }

        public int DepartureStationId { get; set; }
        public int ArrivalStationId { get; set; }
        public TrainStation TrainStation { get; set; }

        [Required] public DateTime DepartureTime { get; set; }

        [Required] public int Duration { get; set; }

        [StringLength(100)] public string Status { get; set; }

    }
}