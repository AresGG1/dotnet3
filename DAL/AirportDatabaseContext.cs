using DAL.Configurations;
using DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL;

public class AirportDatabaseContext : DbContext
{
    public DbSet<Pilot> Pilots { get; set; }
    public DbSet<Aircraft> Aircrafts { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfiguration(new AircraftConfiguration());
        modelBuilder.ApplyConfiguration(new PilotConfiguration());
    }
    
    public AirportDatabaseContext(DbContextOptions<AirportDatabaseContext> options) : base(options)
    {
        
    }

}
