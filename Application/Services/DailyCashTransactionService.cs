using Serilog;

namespace Application.Services
{
    public class DailyCashTransactionService : IDailyCashTransactionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDailyCashTransactionRepository _dailyCashTransactionRepository;
        private readonly IPaymentRepository _paymentRepository;

        private readonly IMapper _mapper;

        public DailyCashTransactionService(IUnitOfWork unitOfWork,
                                            IPaymentRepository paymentRepository,
                                            IDailyCashTransactionRepository dailyCashTransactionRepository,
                                            IMapper mapper)
        {
            _paymentRepository = paymentRepository;
            _unitOfWork = unitOfWork;
            _dailyCashTransactionRepository = dailyCashTransactionRepository;
            _mapper = mapper;
        }


        public async Task<(double, double)> RecordDailyCashTransaction()
        {
            var currentDate = DateTime.Today; // Set to the start of the current day
            var endDate = currentDate.AddDays(1); // Set to the start of the next day

            // Query all relevant payments in one go
            var payments = await _paymentRepository.GetAllPaymentsForDateRange(currentDate, endDate);

            double totalReceived = 0.0;
            double totalRefunded = 0.0;

            foreach (var payment in payments)
            {
                foreach (var ticket in payment.Tickets)
                {
                    // Calculate total amount for the ticket
                    double ticketAmount = ticket.DistanceFare.Price + ticket.Carriage.CarriageType.ServiceCharge + ticket.Seat.SeatType.ServiceCharge;
                    totalReceived += ticketAmount;

                    // Check for cancellation and calculate refund if exists
                    if (ticket.Cancellation != null)
                    {
                        double cancellationFee = ticket.Cancellation.CancellationRule.Fee;
                        totalRefunded += cancellationFee;
                    }
                }
            }

            return (totalReceived, totalRefunded);
        }


        public async Task<PagedList<DailyCashTransactionDto>> GetAllDtoAsync(QueryParams queryParams)
        {
            var query = await _dailyCashTransactionRepository.GetQueryAsync();

            if (!string.IsNullOrEmpty(queryParams.SearchTerm))
                query = query.Where(ct => ct.Date.ToString().Contains(queryParams.SearchTerm.Trim()));

            query = queryParams.Sort switch
            {
                "dateAsc" => query.OrderBy(ct => ct.Date),
                "dateDesc" => query.OrderByDescending(ct => ct.Date),
                "totalReceivedAsc" => query.OrderBy(ct => ct.TotalReceived),
                "totalReceivedDesc" => query.OrderByDescending(ct => ct.TotalReceived),
                "totalRefundedAsc" => query.OrderBy(ct => ct.TotalRefunded),
                "totalRefundedDesc" => query.OrderByDescending(ct => ct.TotalRefunded),
                "createdAtDesc" => query.OrderByDescending(ct => ct.CreatedAt),
                _ => query.OrderBy(ct => ct.CreatedAt)
            };

            var dailyCashTransactionDtoQuery = query.Select(ct => _mapper.Map<DailyCashTransactionDto>(ct));

            return await PagedList<DailyCashTransactionDto>.CreateAsync(dailyCashTransactionDtoQuery, queryParams.PageNumber,
                queryParams.PageSize);
        }

        public Task<DailyCashTransaction> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task AddAsync(DailyCashTransaction t)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(DailyCashTransaction t)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(DailyCashTransaction t)
        {
            throw new NotImplementedException();
        }

        public Task SoftDeleteAsync(DailyCashTransaction t)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> DoWork()
        {
            Log.Information(">>> DailyCashTransactionJob is working");
            var result = await RecordDailyCashTransaction();
            Log.Information(">>> Received: {Received}", result.Item1);
            Log.Information(">>> Refunded: {Refunded}", result.Item2);
            Log.Information(">>> DailyCashTransactionJob is done");


            await _dailyCashTransactionRepository.SaveDailyCashTransaction(result.Item1, result.Item2);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<DailyCashTransactionDto> GetDtoByIdAsync(int id)
        {
            var dailyCashTransaction = await _dailyCashTransactionRepository.GetByIdAsync(id);
            return _mapper.Map<DailyCashTransactionDto>(dailyCashTransaction);
        }

        public async Task<List<DailyCashTransactionDto>> GetAllDtoNoPagingAsync()
        {
            var dailyCashTransaction = await _dailyCashTransactionRepository.GetAllNoPagingAsync();
            return _mapper.Map<List<DailyCashTransactionDto>>(dailyCashTransaction);
        }

        public async Task<List<DailyCashTransactionDto>> GetAllDtoByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            var dailyCashTransaction = await _dailyCashTransactionRepository.GetTransactionsByDateRangeAsync(startDate, endDate);
            return _mapper.Map<List<DailyCashTransactionDto>>(dailyCashTransaction);
        }
    }
}