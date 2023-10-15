using DAL.Interfaces;
using DAL.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Seeding;

public class PilotSeeder : ISeeder<Pilot>
{
    private static readonly List<Pilot> pilots = new()
    {
        new Pilot
        {
            Id = 1,
            Age = 22,
            FirstName = "George",
            LastName = "Tree",
            Rating = 7.1
        },
        new Pilot
        {
            Id = 2,
            Age = 42,
            FirstName = "Mike",
            LastName = "Pound",
            Rating = 9.2
        }
    };
    
    public void Seed(EntityTypeBuilder<Pilot> builder) => builder.HasData(pilots);
    
}