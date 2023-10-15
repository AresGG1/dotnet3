using DAL.Interfaces;

namespace DAL.Models;

public class Pilot : Identifiable
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public int Age { get; set; }
    public double Rating { get; set; }
    public List<Aircraft> Aircrafts { get; set; }
}
