namespace BLL.DTO.Request;

public class AircraftRequest
{
    public string Manufacturer { get; set; }
    public string Model { get; set; }
    public int Year { get; set; }
    public int FlightHours { get; set; }
} 
