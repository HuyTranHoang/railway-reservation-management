using Serilog;

namespace Application.Services
{
    public class DailyCashTransactionService : IDailyCashTransactionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDailyCashTransactionRepository _dailyCashTransactionRepository;
        private readonly IPaymentRepository _paymentRepository;

        public DailyCashTransactionService(IUnitOfWork unitOfWork,
                                            IPaymentRepository paymentRepository,
                                            IDailyCashTransactionRepository dailyCashTransactionRepository)
        {
            _paymentRepository = paymentRepository;
            _unitOfWork = unitOfWork;
            _dailyCashTransactionRepository = dailyCashTransactionRepository;
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
                        var cancellationFee = ticket.Cancellation.CancellationRule.Fee;
                        totalRefunded += cancellationFee;
                    }
                }
            }

            // Save to daily cash transaction table (consider wrapping in a transaction)
            await _dailyCashTransactionRepository.SaveDailyCashTransaction(totalReceived, totalRefunded);
            await _unitOfWork.SaveChangesAsync();

            return (totalReceived, totalRefunded);
        }


        public Task<PagedList<DailyCashTransactionDto>> GetAllDtoAsync(QueryParams queryParams)
        {
            throw new NotImplementedException();
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
    }
}