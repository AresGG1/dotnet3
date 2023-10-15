using DAL.Exceptions;
using DAL.Interfaces;
using DAL.Interfaces.Repositories;
using DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL.Data.Repositories;

public abstract class GenericRepository<T> : IRepository<T> where T : class
{
    protected readonly AirportDatabaseContext DatabaseContext;
    protected readonly DbSet<T> Table;

    public GenericRepository(AirportDatabaseContext databaseContext)
    {
        this.DatabaseContext = databaseContext;
        Table = databaseContext.Set<T>();
    }
    
    public virtual async Task<IEnumerable<T>> GetAsync() => await Table.ToListAsync();

    public virtual async Task<T> GetByIdAsync(int id)
    {
        return await Table.FindAsync(id)
               ?? throw new EntityNotFoundException(
                   GetEntityNotFoundErrorMessage(id));
    }

    public abstract Task<T> GetCompleteEntityAsync(int id);

    public virtual async Task<T> InsertAsync(T entity)
    {
        var ent = await Table.AddAsync(entity);
        
        return ent.Entity;
    }

    public virtual async Task UpdateAsync(T entity)
    {
        Table.Update(entity);
    }

    public virtual async Task DeleteAsync(int id)
    {
        var entity = await GetByIdAsync(id);
        await Task.Run(() => Table.Remove(entity));
    }

    protected static string GetEntityNotFoundErrorMessage(int id) =>
        $"{typeof(T).Name} with id {id} not found.";
    
    //Todo: Refine about selectors | generalize search code
    
    // public delegate string PropertySelectorString<TU>(TU item);
    // public delegate int PropertySelectorInt<TU>(TU item);
    // public delegate double PropertySelectorDouble<TU>(TU item);
    // public delegate IEnumerable<Identifiable> PropertySelector<TD>(TD item);

    // public void SearchByString<TU>(ref IQueryable<TU> source, string str, Func<TU, string> selector)
    // {
    //     if (string.IsNullOrWhiteSpace(str))
    //     {
    //         return;
    //     }
    //     
    //     source = source.Where(entity => selector.Invoke(entity).Equals(str));
    // }
    //
    //     public void SearchByNumber<TU>(ref IQueryable<TU> source, int num, PropertySelectorInt<TU> selector)
    //     {
    //         if (0 == num)
    //         {
    //             return;
    //         }
    //         
    //         source = source.Where(entity => selector(entity).Equals(num));
    //     }
    //     
    //     public void SearchByNumber<TU>(ref IQueryable<TU> source, float num, PropertySelectorDouble<TU> selector)
    //     {
    //         if (0 == num)
    //         {
    //             return;
    //         }
    //         
    //         source = source.Where((entity => (int)selector(entity) == (int)num));
    //     }
}
