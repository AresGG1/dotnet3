using BLL.DTO.Request;
using BLL.DTO.Response;
using DAL.Pagination;
using DAL.Parameters;

namespace BLL.Interfaces.Services;

public interface IPilotService
{
    Task<PagedList<PilotResponse>> GetAsync(PilotParameters parameters);
    
    Task<PilotResponse> GetByIdAsync(int id);

    Task<PilotResponse> InsertAsync(PilotRequest request);

    Task UpdateAsync(PilotRequest request, int id);
    
    Task DeleteAsync(int id);
    Task AssignPilot(PilotAircraftRequest pilotAircraftRequest);
    Task RemoveAssignation(PilotAircraftRequest pilotAircraftRequest);

}
