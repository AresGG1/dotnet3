using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace DAL;

public class AirportDatabaseContextFactory : IDesignTimeDbContextFactory<AirportDatabaseContext>
{
    public AirportDatabaseContext CreateDbContext(string[] args)
    {
        IConfiguration config = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../lab3"))
            .AddJsonFile("appsettings.json")
            .Build();

        string connString = config.GetConnectionString("DefaultConnection");
        
        var optionsBuilder = new DbContextOptionsBuilder<AirportDatabaseContext>();
        optionsBuilder.UseMySql(connString, ServerVersion.AutoDetect(connString));
        
        return new(optionsBuilder.Options);
    }
}