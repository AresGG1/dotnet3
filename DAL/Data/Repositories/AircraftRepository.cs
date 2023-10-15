using DAL.Exceptions;
using DAL.Interfaces.Repositories;
using DAL.Models;
using DAL.Pagination;
using DAL.Parameters;
using Microsoft.EntityFrameworkCore;

namespace DAL.Data.Repositories;

public class AircraftRepository : GenericRepository<Aircraft>, IAircraftRepository
{
    public AircraftRepository(AirportDatabaseContext databaseContext) : base(databaseContext)
    {
    }

    public override async Task<Aircraft> GetCompleteEntityAsync(int id)
    {
        var aircraft = await Table.FirstOrDefaultAsync(a => a.Id == id);

        if (aircraft == null)
        {
            throw new EntityNotFoundException(GetEntityNotFoundErrorMessage(id));
        }
        //Explicit load
        await Table.Entry(aircraft).Collection(a => a.Pilots).LoadAsync();

        return aircraft;
    }

    public async Task<PagedList<Aircraft>> GetAsync(AircraftParameters parameters)
    {   
        IQueryable<Aircraft> source = Table.Include(p => p.Pilots);
        
        SearchByManufacturer(ref source, parameters.Manufacturer);
        SearchByModel(ref source, parameters.Model);
        
        SearchByYear(ref source, parameters.Year);
        SearchByHours(ref source, parameters.FlightHours);
        
        return await PagedList<Aircraft>.ToPagedListAsync(
            source,
            parameters.PageNumber,
            parameters.PageSize);
    }

    private void SearchByManufacturer(ref IQueryable<Aircraft> source, string manufacturer)
    {
        if (String.IsNullOrWhiteSpace(manufacturer))
        {
            return;
        }

        source = source.Where(a => a.Manufacturer.Contains(manufacturer));
    }
    
    private void SearchByModel(ref IQueryable<Aircraft> source, string model)
    {
        if (String.IsNullOrWhiteSpace(model))
        {
            return;
        }

        source = source.Where(a => a.Model.Contains(model));
    }
    
    private void SearchByYear(ref IQueryable<Aircraft> source, int year)
    {
        if (year == 0)
        {
            return;
        }

        source = source.Where(a => a.Year == year);
    }
    
    private void SearchByHours(ref IQueryable<Aircraft> source, int hours)
    {
        if (hours == 0)
        {
            return;
        }

        source = source.Where(a => a.FlightHours == hours);
    }
    
}
