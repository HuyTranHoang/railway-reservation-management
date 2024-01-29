using Mailjet.Client.Resources;

namespace Application.Common.Interfaces.Persistence
{
    public interface IUserRepository
    {
        Task<int> GetUserCountTodayAsync();
         
    }
}