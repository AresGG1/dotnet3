using DAL.Interfaces;

namespace DAL.Models;

public class Aircraft : Identifiable
{
    public int Id { get; set; }
    public string Manufacturer { get; set; }
    public string Model { get; set; }
    public int Year { get; set; }
    public int FlightHours { get; set; }
    public List<Pilot> Pilots { get; set; }
}
