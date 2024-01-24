namespace Application.Common.Interfaces.Services
{
    public interface ILookUpService
    {
        Task<TicketDto> GetByCodeAndPhoneAsync(string code, string email);
    }
}