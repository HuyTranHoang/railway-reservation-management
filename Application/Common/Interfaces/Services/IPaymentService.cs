namespace Application.Common.Interfaces.Services;

public interface IPaymentService : IService<Payment>
{
    Task<PagedList<PaymentDto>> GetAllDtoAsync(PaymentQueryParams queryParams);
    Task<PaymentDto> GetDtoByIdAsync(int id);
}
