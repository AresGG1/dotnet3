namespace DAL.Parameters;

public class AircraftParameters : QueryStringParameters
{
    public string Manufacturer { get; set; }
    public string Model { get; set; }
    public int Year { get; set; }
    public int FlightHours { get; set; }
}
