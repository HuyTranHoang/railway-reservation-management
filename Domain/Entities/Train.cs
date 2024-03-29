using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Common;

namespace Domain.Entities;

public class Train : BaseEntity
{
    public int Id { get; set; }

    [Required] [StringLength(100)] public string Name { get; set; }

    [Required]
    [ForeignKey("TrainCompanyId")]
    public int TrainCompanyId { get; set; }
    public TrainCompany TrainCompany { get; set; }

    [StringLength(100)] public string Status { get; set; }

    public ICollection<Carriage> Carriages { get; set; }
}