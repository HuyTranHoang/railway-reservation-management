using System.ComponentModel.DataAnnotations;
using Domain.Common;

namespace Domain.Entities;

public class TrainCompany : BaseEntity
{
    public int Id { get; set; }

    [Required][StringLength(100)] public string Name { get; set; }

    [StringLength(100)] public string Logo { get; set; }

    [StringLength(100)] public string Status { get; set; }
}