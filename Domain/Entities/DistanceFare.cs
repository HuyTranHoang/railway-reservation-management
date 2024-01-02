using System.ComponentModel.DataAnnotations;
using Domain.Common;

namespace Domain.Entities
{
    public class DistanceFare
    {
        public int Id { get; set; }
        
        public int TrainId { get; set; }
        public Train Train { get; set; }

        public int DepartureStationId { get; set; }
        public int ArrivalStationId { get; set; }
        public TrainStation TrainStation { get; set; }

        [Required] public int Distance { get; set; }

        [Required] public double Price { get; set; }
        
    }
}