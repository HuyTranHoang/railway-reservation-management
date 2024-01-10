using Microsoft.Extensions.Hosting;

namespace Application.Services
{
    public class DailyCashHosted : IHostedService, IDisposable
    {
        private readonly DailyCashTransactionService _dailyCashTransactionService;
        private readonly IDailyCashTransactionRepository _repository;
        private Timer _timer;

        public DailyCashHosted(DailyCashTransactionService dailyCashTransactionService, IDailyCashTransactionRepository repository)
        {
            _dailyCashTransactionService = dailyCashTransactionService;
            _repository = repository;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromMinutes(1));
            return Task.CompletedTask;
        }

        private async void DoWork(object state)
        {
            var result = await _dailyCashTransactionService.RecordDailyCashTransaction();
            double totalReceived = result.Item1;
            double totalRefunded = result.Item2;

            await _repository.SaveDailyCashTransaction(totalReceived, totalRefunded);
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
    }
}