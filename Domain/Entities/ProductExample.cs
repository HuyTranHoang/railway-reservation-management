using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Domain.Common;

namespace Domain.Entities;

public class ProductExample : BaseEntity
{
    [Required]
    [StringLength(100)]
    public string Name { get; set; }

    public string Description { get; set; }

    [Required]
    [Column(TypeName = ("decimal(12,8)"))]
    public decimal Price { get; set; }
}