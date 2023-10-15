using DAL.Interfaces;
using DAL.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Seeding;

public class AircraftSeeder : ISeeder<Aircraft>
{
    private static readonly List<Aircraft> aircrafts = new()
    {
        new Aircraft
        {
            Id = 1,
            Model = "Concord",
            Manufacturer = "BAE Systems",
            Year = 1987,
            FlightHours = 1500
        },
        new Aircraft
        {
            Id = 2,
            Model = "747",
            Manufacturer = "Boeing",
            Year = 2002,
            FlightHours = 200
        }
    };
    
    public void Seed(EntityTypeBuilder<Aircraft> builder) => builder.HasData(aircrafts);
}