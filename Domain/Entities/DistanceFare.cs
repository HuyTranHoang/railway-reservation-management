using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Common;

namespace Domain.Entities
{
    public class DistanceFare : BaseEntity
    {
        public int Id { get; set; }
        
        [Required]
        [ForeignKey("TrainCompanyId")]
        public int TrainCompanyId { get; set; }
        
        public TrainCompany TrainCompany { get; set; }

        [Required] public int Distance { get; set; }

        [Required] public double Price { get; set; }
        
    }
}