using DAL.Cache;
using DAL.Interfaces.Repositories;
using DAL.Models;
using DAL.Pagination;
using DAL.Parameters;
using Microsoft.Extensions.Caching.Memory;

namespace DAL.Data.Repositories;

public class CachedAircraftRepository : IAircraftRepository
{
    public CustomMemoryCache _cacheWraper { get; }
    private readonly AircraftRepository _decorated;
    private readonly IMemoryCache _memoryCache;

    public CachedAircraftRepository(AircraftRepository aircraftRepository, CustomMemoryCache memoryCache)
    {
        _cacheWraper = memoryCache;
        _decorated = aircraftRepository;
        _memoryCache = memoryCache.Cache;
    }

    private string GetAircraftCacheKey(int id, bool full = false)
    {
        return full ? $"aircrafts.full:{id}" : $"aircrafts:{id}";
    }

    public Task<IEnumerable<Aircraft>> GetAsync()
    {
        string cacheKey = $"aircrafts.all";
        
        return _memoryCache.GetOrCreateAsync(cacheKey, entry =>
        {
            entry.AbsoluteExpiration = DateTimeOffset.Now.AddSeconds(10);
            entry.Size = (entry.Value as IEnumerable<Aircraft>)?.Count() ?? 0;
            
            return _decorated.GetAsync();
        });
    }

    public Task<Aircraft> GetByIdAsync(int id)
    {
        string cacheKey = GetAircraftCacheKey(id);
        
        return _memoryCache.GetOrCreateAsync(cacheKey, entry =>
        {
            entry.SlidingExpiration = TimeSpan.FromSeconds(100);
            entry.Size = 1;
            
            return _decorated.GetByIdAsync(id);
        });
    }

    public Task<Aircraft> GetCompleteEntityAsync(int id)
    {
        string cacheKey = GetAircraftCacheKey(id, full: true);
        
        return _memoryCache.GetOrCreateAsync(cacheKey, entry =>
        {
            entry.SlidingExpiration = TimeSpan.FromSeconds(10);
            entry.Size = 1;

            return _decorated.GetCompleteEntityAsync(id);
        });
    }

    public async Task<Aircraft> InsertAsync(Aircraft entity)
    {
        _memoryCache.Remove("aircrafts.all");
        foreach (var key in _cacheWraper.GetParameterKeys())
        {
            _memoryCache.Remove(key);
        }
        
        return await _decorated.InsertAsync(entity);
    }

    public async Task UpdateAsync(Aircraft entity)
    {
        _memoryCache.Remove(GetAircraftCacheKey(entity.Id));
        _memoryCache.Remove(GetAircraftCacheKey(entity.Id, full: true));
        
        await _decorated.UpdateAsync(entity);
    }

    public async Task DeleteAsync(int id)
    {
        _memoryCache.Remove(GetAircraftCacheKey(id));
        _memoryCache.Remove(GetAircraftCacheKey(id, full: true));
        
        await _decorated.DeleteAsync(id);
    }

    public async Task<PagedList<Aircraft>> GetAsync(AircraftParameters parameters)
    {
        string cacheKey = GetListCacheKey(parameters);
        
        return await _memoryCache.GetOrCreateAsync(cacheKey, async entry =>
        {
            _cacheWraper.AddKey(cacheKey);
            entry.AbsoluteExpiration = DateTimeOffset.Now.AddSeconds(10);
        
            var result = await _decorated.GetAsync(parameters);
            entry.Size = result?.Count ?? 0;

            return result;
        });

    }
    
    private string GetListCacheKey(AircraftParameters parameters)
    {
        var keyParts = new List<string>
        {
            $"Manufacturer={parameters.Manufacturer ?? "any"}",
            $"Model={parameters.Model ?? "any"}",
            $"Year={parameters.Year?.ToString() ?? "any"}",
            $"FlightHours={parameters.FlightHours?.ToString() ?? "any"}",
            $"PageNumber={parameters.PageNumber}",
            $"PageSize={parameters.PageSize}"
        };

        return "aircrafts.paged:" + string.Join("_", keyParts.Where(part => !string.IsNullOrEmpty(part)));
    }
}