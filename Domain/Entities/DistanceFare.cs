using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Common;

namespace Domain.Entities
{
    public class DistanceFare : BaseEntity
    {
        public int Id { get; set; }
        
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

        [Required] public int Distance { get; set; }

        [Required] public double Price { get; set; }
        
    }
}