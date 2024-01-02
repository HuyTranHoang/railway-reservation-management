using System.ComponentModel.DataAnnotations;
using Domain.Common;

namespace Domain.Entities;

public class SeatType : BaseEntity
{
    public int Id { get; set; }

    [Required] [StringLength(100)] public string Name { get; set; }

    public double ServiceCharge { get; set; }

    [Required] public string Status { get; set; }
}