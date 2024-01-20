namespace Application.Common.Interfaces.Services
{
    public interface ILookUpService
    {
        Task<TicketDto> GetInfoWithCodeAndEmailAsync(string code, string email);
    }
}