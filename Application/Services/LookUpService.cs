
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
        public async Task<TicketDto> GetByCodeAndPhoneAsync(string code, string phone)
        {
            var ticket = await _ticketRepository.GetByCodeAndPhone(code, phone);

            return _mapper.Map<TicketDto>(ticket);
        }
    }
}