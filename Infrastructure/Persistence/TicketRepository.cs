using Application.Common.Interfaces.Persistence;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence
{
    public class TicketRepository : ITicketRepository
    {
        private readonly ApplicationDbContext _context;

        public TicketRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task Add(Ticket ticket)
        {
            await _context.Tickets.AddAsync(ticket);
        }

        public async Task Delete(Ticket ticket)
        {
            _context.Tickets.Remove(ticket);
            await Task.CompletedTask;
        }

        public Task<List<Ticket>> GetAllNoPagingAsync()
        {
            return _context.Tickets.ToListAsync();
        }

        public List<Ticket> GetAllTickets()
        {
            return _context.Tickets.ToList();
        }

        public async Task<Ticket> GetByCodeAndEmail(string code, string email)
        {
            var ticket = await _context.Tickets
                .Include(d => d.Passenger)
                .Include(d => d.Train)
                .Include(d => d.DistanceFare)
                .Include(d => d.Carriage)
                .Include(d => d.Seat)
                .Include(d => d.Schedule)
                .Include(d => d.Payment)
                .Include(d => d.Payment.AspNetUser)
                .Where(d => d.Code == code)
                .FirstOrDefaultAsync();

            if (ticket != null && ticket.Payment.AspNetUser != null && ticket.Payment.AspNetUser.Email == email)
            {
                return ticket;
            }

            return null;
        }

        public async Task<Ticket> GetByIdAsync(int id)
        {
            return await _context.Tickets.FindAsync(id);
        }

        public async Task<List<Ticket>> GetByIdWithPaymentAsync(int id)
        {
            return await _context.Tickets
                    .Include(t => t.Passenger)
                    .Include(t => t.Train)
                    .Include(t => t.DistanceFare)
                    .Include(t => t.Carriage)
                    .Include(t => t.Seat)
                    .Include(t => t.Schedule)
                    .Include(t => t.Payment)
                    .Include(t => t.Cancellation)
                .Where(t => t.PaymentId == id)
                .ToListAsync();
        }

        public async Task<IQueryable<Ticket>> GetQueryAsync()
        {
            return await Task.FromResult(_context.Tickets.AsQueryable());
        }

        public async Task<IQueryable<Ticket>> GetQueryWithRelationshipTableAsync()
        {
            return await Task.FromResult(
                _context.Tickets
                    .Include(t => t.Passenger)
                    .Include(t => t.Train)
                    .Include(t => t.DistanceFare)
                    .Include(t => t.Carriage)
                    .Include(t => t.Seat)
                    .Include(t => t.Schedule)
                    .Include(t => t.Payment)
                    .AsQueryable());
        }


        public async Task SoftDelete(Ticket ticket)
        {
            ticket.IsDeleted = true;
            _context.Entry(ticket).State = EntityState.Modified;
            await Task.CompletedTask;
        }

        public async Task Update(Ticket ticket)
        {
            _context.Entry(ticket).State = EntityState.Modified;
            await Task.CompletedTask;
        }
        
        public async Task<int> GetTicketCountTodayAsync()
        {
            DateTime today = DateTime.UtcNow.Date;

            int ticketCount = await _context.Tickets
                .CountAsync(ticket => ticket.CreatedAt.Date == today);

            return ticketCount;
        }

        // public async Task<double> GetTicketPriceTodayAsync()
        // {
        //     DateTime today = DateTime.UtcNow.Date;

        //     double totalPrice = await _context.Tickets
        //         .Where(ticket => ticket.CreatedAt.Date == today)
        //         .SumAsync(ticket => ticket.Price);

        //     return totalPrice;
        // }

        public async Task<double> GetTicketPriceSumByDateAsync(DateTime date)
        {
            return await _context.Tickets
                .Where(ticket => ticket.CreatedAt.Date == date.Date)
                .SumAsync(ticket => ticket.Price);
        }

        public async Task<int> GetSeatsBookedInSchedule(int scheduleId)
        {
            return await _context.Tickets
                .Where(ticket => ticket.ScheduleId == scheduleId)
                .CountAsync();
        }
    }
}