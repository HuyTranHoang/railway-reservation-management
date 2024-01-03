using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Common;

namespace Domain.Entities
{
    public class Payment : BaseEntity
    {
        public int Id { get; set; }

        [Required]
        [ForeignKey("PassengerId")]
        public int PassengerId { get; set; }
        public Passenger Passenger { get; set; }

        [Required]
        [ForeignKey("TicketId")]
        public int TicketId { get; set; }
        public Ticket Ticket { get; set; }

        [StringLength(100)] public string Status { get; set; }

        
    }
}