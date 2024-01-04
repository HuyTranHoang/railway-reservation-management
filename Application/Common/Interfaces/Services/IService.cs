namespace Application.Common.Interfaces.Services;

public interface IService<T> where T : class
{
    Task<T> GetByIdAsync(int id);
    Task AddAsync(T t);
    Task UpdateAsync(T t);
    Task DeleteAsync(T t);
    Task SoftDeleteAsync(T t);
}