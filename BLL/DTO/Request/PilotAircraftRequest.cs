using System.ComponentModel.DataAnnotations;

namespace BLL.DTO.Request;

public class PilotAircraftRequest
{
    public int PilotId { get; set; }
    public int AircraftId { get; set; }
}