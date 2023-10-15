using DAL.Models;
using DAL.Pagination;
using DAL.Parameters;

namespace DAL.Interfaces.Repositories;

public interface IPilotRepository : IRepository<Pilot>
{
    Task<PagedList<Pilot>> GetAsync(PilotParameters parameters);
}
