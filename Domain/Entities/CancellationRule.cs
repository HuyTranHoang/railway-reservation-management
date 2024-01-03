using System.ComponentModel.DataAnnotations;
using Domain.Common;

namespace Domain.Entities
{
    public class CancellationRule : BaseEntity
    {
        public int Id { get; set; }
        
        [Required] public int DepartureDateDifference { get; set; }

        [Required] public double Fee { get; set; }

        [StringLength(100)] public string Status { get; set; }

    }
}