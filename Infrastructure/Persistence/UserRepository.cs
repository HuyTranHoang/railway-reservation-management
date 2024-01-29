using Application.Common.Interfaces.Persistence;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<int> GetUserCountTodayAsync()
        {
        DateTime today = DateTime.UtcNow.Date;

        int userCount = await _context.Users
            .CountAsync(user => user.CreatedAt.Date == today);

        return userCount;
        }
    }
}