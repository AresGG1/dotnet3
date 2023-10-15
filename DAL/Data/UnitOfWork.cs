using DAL.Interfaces;
using DAL.Interfaces.Repositories;

namespace DAL.Data;

public class UnitOfWork : IUnitOfWork
{
    private readonly AirportDatabaseContext _databaseContext;

    public UnitOfWork(
        IAircraftRepository aircraftRepository,
        IPilotRepository pilotRepository,
        AirportDatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
        AircraftRepository = aircraftRepository;
        PilotRepository = pilotRepository;
    }
    public IAircraftRepository AircraftRepository { get; }
    public IPilotRepository PilotRepository { get; }
    
    public async Task SaveChangesAsync()
    {
        await _databaseContext.SaveChangesAsync();
    }
}
