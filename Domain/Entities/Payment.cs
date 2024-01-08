using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Common;

namespace Domain.Entities
{
    public class Payment : BaseEntity
    {
        [Required] public int Id { get; set; }
        [Required] public string UserId { get; set; }
        [StringLength(100)] public string Status { get; set; }
    }
}