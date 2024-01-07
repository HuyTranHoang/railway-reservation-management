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
        public void Add(Ticket ticket)
        {
            _context.Tickets.Add(ticket);
        }

        public void Delete(Ticket ticket)
        {
            _context.Tickets.Remove(ticket);
        }

        public async Task<Ticket> GetByIdAsync(int id)
        {
            return await _context.Tickets.FindAsync(id);
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
                .AsQueryable());
        }
        
        public void SoftDelete(Ticket ticket)
        {
            ticket.IsDeleted = true;
            _context.Entry(ticket).State = EntityState.Modified;
        }

        public void Update(Ticket ticket)
        {
            _context.Entry(ticket).State = EntityState.Modified;
        }
    }
}