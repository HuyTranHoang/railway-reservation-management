using Domain.Exceptions;

namespace Application.Services;

public class PaymentService : IPaymentService
{
    private readonly IMapper _mapper;
    private readonly IPaymentRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public PaymentService(IPaymentRepository repositoy, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _repository = repositoy;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task AddAsync(Payment payment)
    {
        await _repository.Add(payment);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task DeleteAsync(Payment payment)
    {
        await _repository.Delete(payment);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<PagedList<PaymentDto>> GetAllDtoAsync(PaymentQueryParams queryParams)
    {
        var query = await _repository.GetQueryWithAspNetUserAsync();

        if (queryParams.CreatedAt != DateTime.MinValue)
        {
            query = query.Where(t => t.CreatedAt.Date == queryParams.CreatedAt.Date);
        }

        if (!string.IsNullOrEmpty(queryParams.SearchTerm))
            query = query.Where(t => t.AspNetUser.Email.Contains(queryParams.SearchTerm.Trim()));

        query = queryParams.Sort switch
        {
            "emailAsc" => query.OrderBy(t => t.AspNetUser.Email),
            "emailDesc" => query.OrderByDescending(t => t.AspNetUser.Email),
            "createdAtDesc" => query.OrderByDescending(t => t.CreatedAt),
            _ => query.OrderBy(t => t.CreatedAt)
        };


        var paymentsDtoQuery = query.Select(t => _mapper.Map<PaymentDto>(t));

        return await PagedList<PaymentDto>.CreateAsync(paymentsDtoQuery, queryParams.PageNumber,
            queryParams.PageSize);
    }

    public async Task<Payment> GetByIdAsync(int id)
    {
        return await _repository.GetByIdAsync(id);
    }

    public async Task<PaymentDto> GetDtoByIdAsync(int id)
    {
        var payment = await _repository.GetPaymentWithAspNetUserByIdAsync(id);
        return _mapper.Map<PaymentDto>(payment);
    }

    public async Task SoftDeleteAsync(Payment payment)
    {
        await _repository.SoftDelete(payment);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task UpdateAsync(Payment payment)
    {
        var paymentInDb = await _repository.GetByIdAsync(payment.Id);

        if (paymentInDb == null) throw new NotFoundException(nameof(Payment), payment.Id);

        paymentInDb.AspNetUserId = payment.AspNetUserId;
        paymentInDb.Status = payment.Status;
        paymentInDb.UpdatedAt = DateTime.Now;

        await _unitOfWork.SaveChangesAsync();
    }
}
