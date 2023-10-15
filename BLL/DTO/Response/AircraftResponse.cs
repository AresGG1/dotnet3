namespace BLL.DTO.Response;

public class AircraftResponse
{
    public int Id { get; set; }
    public string Manufacturer { get; set; }
    public string Model { get; set; }
    public int Year { get; set; }
    public int FlightHours { get; set; }
    public List<PilotResponse> Pilots { get; set; }
}
