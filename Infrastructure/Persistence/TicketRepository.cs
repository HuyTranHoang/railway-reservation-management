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
    }
}