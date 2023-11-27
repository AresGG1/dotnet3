using DAL.Exceptions;
using DAL.Interfaces.Repositories;
using DAL.Models;
using DAL.Pagination;
using DAL.Parameters;
using Microsoft.EntityFrameworkCore;

namespace DAL.Data.Repositories;

public class PilotRepository : GenericRepository<Pilot>, IPilotRepository
{
    public PilotRepository(AirportDatabaseContext databaseContext) : base(databaseContext)
    {
    }

    public override async Task<Pilot> GetCompleteEntityAsync(int id)
    {
        //Eager loading
        var pilot = await Table
            .Include(p => p.Aircrafts)
            .FirstOrDefaultAsync(a => a.Id == id);

        if (pilot == null)
        {
            throw new EntityNotFoundException(GetEntityNotFoundErrorMessage(id));
        }
        
        return pilot;
    }

    public List<AircraftPilot> GetLinkEntities()
    {
        var query = Table
            .Include(p => p.Aircrafts)
            .SelectMany(
                p => p.Aircrafts,
                (table, aircraft) => new AircraftPilot
                {
                    PilotsId = table.Id,
                    AircraftsId = aircraft.Id,
                }
            )
            .ToList();

        return query.ToList();
    }

    public async Task<PagedList<Pilot>> GetAsync(PilotParameters pilotParameters)
    {
        IQueryable<Pilot> source = Table.Include(p => p.Aircrafts);

        // source = source.Where(p => p.FirstName == pilotParameters.FirstName);
        SearchByFirstName(ref source, pilotParameters.FirstName);
        SearchByLastName(ref source, pilotParameters.LastName);
        
        SearchByAge(ref source, pilotParameters.Age);
        SearchByRating(ref source, pilotParameters.Rating);
        
        SearchByAircraftId(ref source, pilotParameters.AircraftId);
        
        return await PagedList<Pilot>.ToPagedListAsync(
            source,
            pilotParameters.PageNumber,
            pilotParameters.PageSize);
        
    }
    
    private void SearchByFirstName(ref IQueryable<Pilot> source, string firstName)
    {
        if (String.IsNullOrWhiteSpace(firstName))
        {
            return;
        }

        source = source.Where(a => a.FirstName.Contains(firstName));
    }
    
    private void SearchByLastName(ref IQueryable<Pilot> source, string lastName)
    {
        if (String.IsNullOrWhiteSpace(lastName))
        {
            return;
        }

        source = source.Where(a => a.LastName.Contains(lastName));
    }
    
    private void SearchByAge(ref IQueryable<Pilot> source, int? age)
    {
        if (age is null)
        {
            return;
        }

        source = source.Where(a => a.Age == age);
    }
    
    private void SearchByRating(ref IQueryable<Pilot> source, double? rating)
    {
        if (rating is null)
        {
            return;
        }

        source = source.Where(a => Math.Abs(a.Rating - rating.Value) < 1);
    }
    private void SearchByAircraftId(ref IQueryable<Pilot> source, int? aircraftId)
    {
        if (aircraftId is null)
        {
            return;
        }

        source = source.Where(p => p.Aircrafts.Any(a => a.Id == aircraftId.Value));
    }
    
}
