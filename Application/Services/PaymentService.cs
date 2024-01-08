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
        _repository.Add(payment);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task DeleteAsync(Payment payment)
    {
        _repository.Delete(payment);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<PagedList<PaymentDto>> GetAllDtoAsync(PaymentQueryParams queryParams)
    {
        var query = await _repository.GetQueryAsync();

        if (queryParams.UserId != null)
        {
            query = query.Where(t => t.UserId == queryParams.UserId);
        }

        if (!string.IsNullOrEmpty(queryParams.SearchTerm))
            query = query.Where(t => t.UserId.Contains(queryParams.SearchTerm));

        query = queryParams.Sort switch
        {
            "userIdAsc" => query.OrderBy(t => t.UserId),
            "userIdDesc" => query.OrderByDescending(t => t.UserId),
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
        var paymentDto = await _repository.GetByIdAsync(id);
        return _mapper.Map<PaymentDto>(paymentDto);
    }

    public async Task SoftDeleteAsync(Payment payment)
    {
        _repository.SoftDelete(payment);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task UpdateAsync(Payment payment)
    {
        var paymentInDb = await _repository.GetByIdAsync(payment.Id);

        if (paymentInDb == null) throw new NotFoundException(nameof(Payment), payment.Id);

        paymentInDb.UserId = payment.UserId;
        paymentInDb.UpdatedAt = DateTime.Now;

        await _unitOfWork.SaveChangesAsync();
    }
}
