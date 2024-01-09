using Microsoft.Extensions.Hosting;

namespace Application.Services
{
    public class DailyCashTransactionService : IHostedService, IDisposable
    {
        private readonly IDailyCashTransactionRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private Timer _timer;

        public DailyCashTransactionService(IDailyCashTransactionRepository repository, IUnitOfWork unitOfWork, Timer timer)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _timer = timer;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromDays(1));

            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            RecordDailyCashTransaction();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }

        private void RecordDailyCashTransaction()
        {
            var 

            using (var context)
            {
                var totalReceived = 0;
                var totalRefunded = 0;
            };
            

        
        }

    }

}