using System.ComponentModel.DataAnnotations;
using Domain.Common;

namespace Domain.Entities
{
    public class Carriage : BaseEntity
    {
        public int Id { get; set; }

        [Required] [StringLength(100)] public string Name { get; set; }

        public int TrainId { get; set; }
        public Train Train { get; set; }

        [Required] public int NumberOfCompartment { get; set; }

        [Required] public double ServiceCharge { get; set; }

        [Required] [StringLength(100)] public int Status { get; set; }

    }
}