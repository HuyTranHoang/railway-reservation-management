using System.ComponentModel.DataAnnotations;

namespace Application.Common.Models;

public class PaymentTransactionDto
{
    public List<PaymentPassengerDto> Passengers { get; set; }
    public List<PaymentTicketDto> Tickets { get; set; }
    [Required]
    public int PaymentId { get; set; }
    [Required]
    public int TrainId { get; set; }
    [Required]
    public int ScheduleId { get; set; }
}

public class PaymentPassengerDto
{
    [Required] [StringLength(256)] public string FullName { get; set; }
    [Required] [StringLength(30)] public string CardId { get; set; }
    public int Age { get; set; }
    [StringLength(256)] public string Gender { get; set; }
    [Required] [StringLength(256)] public string Phone { get; set; }
    [EmailAddress] [StringLength(256)] public string Email { get; set; }
}

public class PaymentTicketDto
{
    public int CarriageId { get; set; }
    public int SeatId { get; set; }
}