using DAL.Models;
using DAL.Pagination;
using DAL.Parameters;

namespace DAL.Interfaces.Repositories;

public interface IAircraftRepository : IRepository<Aircraft>
{
    Task<PagedList<Aircraft>> GetAsync(AircraftParameters parameters);
}