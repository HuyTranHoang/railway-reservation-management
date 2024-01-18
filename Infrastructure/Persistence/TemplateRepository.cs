using Application.Common.Interfaces.Persistence;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence
{
    public class TemplateRepository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;

        public TemplateRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task Add(T t)
        {
            await _context.Set<T>().AddAsync(t);
        }

        public async Task Delete(T t)
        {
            _context.Set<T>().Remove(t);
            await Task.CompletedTask;
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task<IQueryable<T>> GetQueryAsync()
        {
            return await Task.FromResult(_context.Set<T>().AsQueryable());
        }

        public async Task SoftDelete(T t)
        {
            var isDeletedProperty = t.GetType().GetProperty("IsDeleted");

            if (isDeletedProperty != null && isDeletedProperty.PropertyType == typeof(bool))
            {
                isDeletedProperty.SetValue(t, true);
                _context.Entry(t).State = EntityState.Modified;
            }
            else
            {
                throw new InvalidOperationException("This entity does not support soft delete.");
            }
            await Task.CompletedTask;
        }

        public async Task Update(T t)
        {
            _context.Entry(t).State = EntityState.Modified;
            await Task.CompletedTask;
        }
    }

}