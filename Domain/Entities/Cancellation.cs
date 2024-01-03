using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Common;

namespace Domain.Entities
{
    public class Cancellation : BaseEntity
    {
        public int Id { get; set; }
        
        [Required]
        [ForeignKey("TicketId")]
        public int TicketId { get; set; }
        public Ticket Ticket { get; set; }
        
        [Required]
        [ForeignKey("CancellationRuleId")]
        public int CancellationRuleId { get; set; }
        public CancellationRule CancellationRule { get; set; }

        [Required] [StringLength(450)] public string Reason { get; set; }

        [Required] public double RefundAmount { get; set; }

        [StringLength(100)] public string Status { get; set; }


    }
}