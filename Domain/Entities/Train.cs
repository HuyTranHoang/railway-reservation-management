using System.ComponentModel.DataAnnotations;
using Domain.Common;

namespace Domain.Entities;

public class Train : BaseEntity
{
    public int Id { get; set; }

    [Required] [StringLength(100)] public string Name { get; set; }

    public int TrainCompanyId { get; set; }
    public TrainCompany TrainCompany { get; set; }

    [StringLength(100)] public string Status { get; set; }
}