using System.ComponentModel.DataAnnotations;
using Domain.Common;

namespace Domain.Entities;

public class Passenger : BaseEntity
{
    public int Id { get; set; }

    [Required] [StringLength(256)] public string FullName { get; set; }

    [Required] [StringLength(30)] public string CardId { get; set; }

    public int Age { get; set; }

    [StringLength(256)] public string Gender { get; set; }

    [Required] [StringLength(256)] public string Phone { get; set; }

    [StringLength(256)] public string Email { get; set; }
}