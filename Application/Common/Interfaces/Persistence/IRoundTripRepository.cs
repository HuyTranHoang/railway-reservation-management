namespace Application.Common.Interfaces.Persistence
{
    public interface IRoundTripRepository : IRepository<RoundTrip>
    {
        Task<IQueryable<RoundTrip>> GetQueryWithTrainCompanyAsync();
        Task<RoundTrip> GetByIdWithTrainCompanyAsync(int id);

        Task<RoundTrip> GetByTrainCompanyIdAsync(int trainCompanyId);
    }
}