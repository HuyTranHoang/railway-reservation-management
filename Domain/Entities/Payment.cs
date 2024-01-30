using System.ComponentModel.DataAnnotations;
using Domain.Common;

namespace Domain.Entities
{
    public class Payment : BaseEntity
    {
        [Required] public int Id { get; set; }
        [Required] public string AspNetUserId { get; set; }
        public ApplicationUser AspNetUser { get; set; }

        [Required]
        [StringLength(100)]
        public string TransactionId { get; set; }

        [StringLength(100)]
        public string Status { get; set; }
        public ICollection<Ticket> Tickets { get; set; }
    }
}