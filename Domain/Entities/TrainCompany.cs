using System.ComponentModel.DataAnnotations;
using Domain.Common;

namespace Domain.Entities;

public class TrainCompany : BaseEntity
{
    public int Id { get; set; }

    [Required] [StringLength(100)] public string Name { get; set; }
}