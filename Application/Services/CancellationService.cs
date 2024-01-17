
using Domain.Exceptions;

namespace Application.Services
{
    public class CancellationService : ICancellationService
    {
        private readonly ICancellationRepository _repository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public CancellationService(ICancellationRepository repository, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task AddAsync(Cancellation cancellation)
        {
            await _repository.Add(cancellation);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(Cancellation cancellation)
        {
            await _repository.Delete(cancellation);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<PagedList<CancellationDto>> GetAllDtoAsync(CancellationQueryParams queryParams)
        {
            var query = await _repository.GetQueryWithCancellationRuleAndTicketAsync();

            if (queryParams.TicketId != 0)
            {
                query = query.Where(t => t.TicketId == queryParams.TicketId);
            }

            if (queryParams.CancellationRuleId != 0)
            {
                query = query.Where(t => t.CancellationRuleId == queryParams.CancellationRuleId);
            }

            if (!string.IsNullOrEmpty(queryParams.SearchTerm))
                query = query.Where(p => p.Ticket.Code.Contains(queryParams.SearchTerm.Trim()));

            query = queryParams.Sort switch
            {
                "ticketCodeAsc" => query.OrderBy(p => p.Ticket.Code),
                "ticketCodeDesc" => query.OrderByDescending(p => p.Ticket.Code),
                "departureDateDifferenceAsc" => query.OrderBy(p => p.CancellationRule.DepartureDateDifference),
                "departureDateDifferenceDesc" => query.OrderByDescending(p => p.CancellationRule.DepartureDateDifference),
                _ => query.OrderBy(p => p.CreatedAt)
            };

            var cancellationDtoQuery = query.Select(p => _mapper.Map<CancellationDto>(p));

            return await PagedList<CancellationDto>.CreateAsync(cancellationDtoQuery, queryParams.PageNumber,
                queryParams.PageSize);
        }

        public async Task<Cancellation> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<CancellationDto> GetDtoByIdAsync(int id)
        {
            var cancellation = await _repository.GetByIdAsync(id);
            return _mapper.Map<CancellationDto>(cancellation);
        }

        public async Task SoftDeleteAsync(Cancellation cancellation)
        {
            await _repository.SoftDelete(cancellation);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateAsync(Cancellation cancellation)
        {
            var cancellationInDb = await _repository.GetByIdAsync(cancellation.Id);

            if (cancellationInDb == null) throw new NotFoundException(nameof(Cancellation), cancellation.Id);

            cancellationInDb.TicketId = cancellation.TicketId;
            cancellationInDb.CancellationRuleId = cancellation.CancellationRuleId;
            cancellationInDb.Reason = cancellation.Reason;
            cancellationInDb.Status = cancellation.Status;
            cancellationInDb.UpdatedAt = DateTime.Now;

            await _repository.Update(cancellationInDb);

            await _unitOfWork.SaveChangesAsync();
        }
    }
}