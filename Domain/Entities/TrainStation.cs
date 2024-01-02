using System.ComponentModel.DataAnnotations;
using Domain.Common;

namespace Domain.Entities
{
    public class TrainStation : BaseEntity
    {
        public int Id { get; set; }
        
        [Required] [StringLength(100)] public string Name { get; set; }

        [Required] [StringLength(450)] public string Address { get; set; }

        [Required] public int CoordinateValue { get; set; }

        [StringLength(100)] public string Status { get; set; }

    }
}