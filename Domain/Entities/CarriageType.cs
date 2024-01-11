using System.ComponentModel.DataAnnotations;
using Domain.Common;

namespace Domain.Entities
{
    public class CarriageType : BaseEntity
    {
        public int Id { get; set; }

        [Required][StringLength(100)] public string Name { get; set; }

        public double ServiceCharge { get; set; }

        [Required][StringLength(450)] public string Description { get; set; }

        public string Status { get; set; }
    }
}