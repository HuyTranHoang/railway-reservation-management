using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Common;

public class BaseEntity
{
    public int Id { get; set; }

    [Column(Order = 997)]
    public bool IsDeleted { get; set; } = false;

    [Column(Order = 998)]
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    [Column(Order = 999)]
    public DateTime? UpdatedAt { get; set; }
}