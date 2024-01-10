namespace Application.Common.Models.QueryParams
{
    public class CancellationQueryParams : QueryParams
    {
        public int TicketId { get; set; }

        public int CancellationRuleId { get; set; }
    }
}