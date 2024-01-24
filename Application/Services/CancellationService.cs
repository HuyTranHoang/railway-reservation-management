
using Domain.Exceptions;

namespace Application.Services
{
    public class CancellationService : ICancellationService
    {
        private readonly ICancellationRepository _repository;
        private readonly ITicketRepository _ticketRepository;
        private readonly IScheduleRepository _scheduleRepository;
        private readonly ICancellationRuleRepository _cancellationRuleRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public CancellationService(ICancellationRepository repository,
            ITicketRepository ticketRepository,
            IScheduleRepository scheduleRepository,
            ICancellationRuleRepository cancellationRuleRepository,
            IMapper mapper,
            IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _ticketRepository = ticketRepository;
            _scheduleRepository = scheduleRepository;
            _cancellationRuleRepository = cancellationRuleRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task AddAsync(Cancellation cancellation)
        {
            var today = DateTime.UtcNow;

            var ticket = await _ticketRepository.GetByIdAsync(cancellation.TicketId);
            if (ticket == null) throw new NotFoundException(nameof(Ticket), cancellation.TicketId);

            var schedule = await _scheduleRepository.GetByIdAsync(ticket.ScheduleId);
            if (schedule == null) throw new NotFoundException(nameof(Schedule), ticket.ScheduleId);

            var isCancelled = await _repository.GetByTicketIdAsync(cancellation.TicketId);
            if (isCancelled != null) throw new BadRequestException(400,"Ticket has been cancelled");

            if (schedule.DepartureTime < today)
            {
                throw new BadRequestException(400,"Cannot cancel ticket after departure time");
            }

            var dateDifference = (schedule.DepartureTime - today).Days;

            var cancellationRule = await _cancellationRuleRepository.GetByDifferenDateAsync(dateDifference);

            cancellation.CancellationRuleId = cancellationRule.Id;

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
                "cancellationRuleFeeAsc" => query.OrderBy(p => p.CancellationRule.Fee),
                "cancellationRuleFeeDesc" => query.OrderByDescending(p => p.CancellationRule.Fee),
                "cancellationRuleDepartureDateDifferenceAsc" => query.OrderBy(p => p.CancellationRule.DepartureDateDifference),
                "cancellationRuleDepartureDateDifferenceDesc" => query.OrderByDescending(p => p.CancellationRule.DepartureDateDifference),
                "reasonAsc" => query.OrderBy(p => p.Reason),
                "reasonDesc" => query.OrderByDescending(p => p.Reason),
                "createdAtDesc" => query.OrderByDescending(cr => cr.CreatedAt),
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