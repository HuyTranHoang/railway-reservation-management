namespace Application.Common.Interfaces.Persistence;

public interface IRepository<T> where T : class
{
    Task<IQueryable<T>> GetQueryAsync();
    Task<T> GetByIdAsync(int id);
    void Add(T t);
    void Update(T t);
    void Delete(T t);
    void SoftDelete(T t);
    Task AddAsync<T>(T t) where T : class;
}