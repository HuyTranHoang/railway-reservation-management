namespace Application.Common.Interfaces.Persistence
{
    public interface IRoundTripRepository : IRepository<RoundTrip>
    {
         Task<IQueryable<RoundTrip>> GetQueryWithTrainCompanyAsync();
    }
}