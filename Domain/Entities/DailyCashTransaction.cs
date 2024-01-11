using System.ComponentModel.DataAnnotations;
using Domain.Common;

namespace Domain.Entities
{
    public class DailyCashTransaction : BaseEntity
    {
        public int Id { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public double TotalReceived { get; set; }

        [Required]
        public double TotalRefunded { get; set; }
    }
}