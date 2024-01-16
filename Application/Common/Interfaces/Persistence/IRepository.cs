namespace Application.Common.Interfaces.Persistence;

public interface IRepository<T> where T : class
{
    Task<IQueryable<T>> GetQueryAsync();
    Task<T> GetByIdAsync(int id);
    Task Add(T t);
    Task Update(T t);
    Task Delete(T t);
    Task SoftDelete(T t);
}