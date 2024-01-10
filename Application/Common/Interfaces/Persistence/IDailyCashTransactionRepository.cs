namespace Application.Common.Interfaces.Persistence
{
    public interface IDailyCashTransactionRepository : IRepository<DailyCashTransaction>
    {
         Task SaveDailyCashTransaction(double totalReceived, double totalRefunded);
    }
}