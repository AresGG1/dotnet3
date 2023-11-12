namespace DAL.Interfaces.Repositories;

public interface IRepository<T> where T : class
{
    Task<T> GetByIdAsync(int id);

    Task<T> GetCompleteEntityAsync(int id);

    Task<T> InsertAsync(T entity);

    Task UpdateAsync(T entity);

    Task DeleteAsync(int id);
}