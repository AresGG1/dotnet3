using DAL.Interfaces.Repositories;

namespace DAL.Interfaces;

public interface IUnitOfWork
{
    public IAircraftRepository AircraftRepository { get; }
    public IPilotRepository PilotRepository { get; }
    public Task SaveChangesAsync();

}
