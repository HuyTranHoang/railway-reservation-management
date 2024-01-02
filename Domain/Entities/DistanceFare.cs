using System.ComponentModel.DataAnnotations;
using Domain.Common;

namespace Domain.Entities
{
    public class DistanceFare : BaseEntity
    {
        public int Id { get; set; }
        
        [Required] public int TrainId { get; set; }
        public Train Train { get; set; }

        [Required] public int DepartureStationId { get; set; }
        public TrainStation DepartureStation { get; set; }

        [Required] public int ArrivalStationId { get; set; }
        public TrainStation ArrivalStation { get; set; }

        [Required] public int Distance { get; set; }

        [Required] public double Price { get; set; }
        
    }
}