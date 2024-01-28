namespace Application.Common.Interfaces.Services
{
    public interface ILookUpService
    {
        Task<TicketDto> GetByCodeAndEmailAsync(string code, string email);
    }
}