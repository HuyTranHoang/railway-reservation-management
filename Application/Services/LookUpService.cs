
namespace Application.Services
{
    public class LookUpService : ILookUpService
    {
        private readonly IMapper _mapper;
        private readonly ITicketRepository _ticketRepository;

        public LookUpService(IMapper mapper,
                            ITicketRepository ticketRepository)
        {
            _mapper = mapper;
            _ticketRepository = ticketRepository;
        }
        public async Task<TicketDto> GetInfoWithCodeAndEmailAsync(string code, string email)
        {
            var ticket = await _ticketRepository.GetByCodeAndEmail(code, email);

            return _mapper.Map<TicketDto>(ticket);
        }
    }
}