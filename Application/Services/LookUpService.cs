namespace Application.Services
{
    public class LookUpService : ILookUpService
    {
        private readonly IMapper _mapper;
        private readonly ICancellationRepository _cancellationRepository;
        private readonly ITicketRepository _ticketRepository;

        public LookUpService(IMapper mapper,
            ICancellationRepository cancellationRepository,
            ITicketRepository ticketRepository)
        {
            _mapper = mapper;
            _cancellationRepository = cancellationRepository;
            _ticketRepository = ticketRepository;
        }

        public async Task<TicketDto> GetByCodeAndEmailAsync(string code, string email)
        {
            var ticket = await _ticketRepository.GetByCodeAndEmail(code, email);

            if (ticket is null) return null;

            var ticketDto = _mapper.Map<TicketDto>(ticket);

            ticketDto.IsCancel = await _cancellationRepository.IsTicketCancelledAsync(ticket.Id);

            return ticketDto;
        }
    }
}