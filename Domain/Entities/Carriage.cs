using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Common;

namespace Domain.Entities
{
    public class Carriage : BaseEntity
    {
        public int Id { get; set; }

        [Required] [StringLength(100)] public string Name { get; set; }

        [Required]
        [ForeignKey("TrainId")]
         public int TrainId { get; set; }
        public Train Train { get; set; }

        [Required] public int NumberOfCompartment { get; set; }

        [Required] public double ServiceCharge { get; set; }

        [StringLength(100)] public string Status { get; set; }

    }
}