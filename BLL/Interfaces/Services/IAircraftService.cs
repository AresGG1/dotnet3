using BLL.DTO.Request;
using BLL.DTO.Response;
using DAL.Pagination;
using DAL.Parameters;

namespace BLL.Interfaces.Services;

public interface IAircraftService
{

    Task<PagedList<AircraftResponse>> GetAsync(AircraftParameters parameters);
    
    Task<AircraftResponse> GetByIdAsync(int id);

    Task<AircraftResponse> InsertAsync(AircraftRequest request);

    Task UpdateAsync(AircraftRequest request, int id);
    
    Task DeleteAsync(int id);
}
