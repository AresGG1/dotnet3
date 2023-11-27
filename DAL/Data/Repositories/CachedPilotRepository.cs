using DAL.Converters;
using DAL.Interfaces.Repositories;
using DAL.Models;
using DAL.Pagination;
using DAL.Parameters;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace DAL.Data.Repositories;

public class CachedPilotRepository : IPilotRepository
{
    private readonly IDistributedCache _cache;
    private readonly PilotRepository _decorated;
    public CachedPilotRepository(IDistributedCache cache, PilotRepository pilotRepository)
    {
        _cache = cache;
        _decorated = pilotRepository;
    }
    
    private string GetPilotCacheKey(int id, bool full = false)
    {
        return full ? $"pilots.full:{id}" : $"pilots:{id}";
    }
    
    public async Task<Pilot> GetByIdAsync(int id)
    {
        string cacheKey = GetPilotCacheKey(id);
        
        string? cachedPilot = await _cache.GetStringAsync(cacheKey);

        Pilot pilot;
        
        if (string.IsNullOrEmpty(cachedPilot))
        {
            pilot = await _decorated.GetByIdAsync(id);
            await _cache.SetStringAsync(
                cacheKey,
                JsonConvert.SerializeObject(pilot)
            );

            return pilot;
        }

        pilot = JsonConvert.DeserializeObject<Pilot>(cachedPilot);

        return pilot;
    }

    public async Task<Pilot> GetCompleteEntityAsync(int id)
    {
        string cacheKey = GetPilotCacheKey(id, true);
        
        string? cachedPilot = await _cache.GetStringAsync(cacheKey);

        Pilot pilot;
        
        if (string.IsNullOrEmpty(cachedPilot))
        {
            pilot = await _decorated.GetCompleteEntityAsync(id);
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(10) // Set absolute expiration to 10 seconds
            };

            
            await _cache.SetStringAsync(
                cacheKey,
                JsonConvert.SerializeObject(pilot),
                options
            );

            return pilot;
        }

        pilot = JsonConvert.DeserializeObject<Pilot>(cachedPilot);

        return pilot;
    }

    public async Task<Pilot> InsertAsync(Pilot entity)
    {
        PurgeCache();
        return await _decorated.InsertAsync(entity);
    }

    public async Task UpdateAsync(Pilot entity)
    {
        PurgeCache(entity.Id);
        await _decorated.UpdateAsync(entity);
    }

    public async Task DeleteAsync(int id)
    {
        PurgeCache(id);
        
        await _decorated.DeleteAsync(id);
    }

    public async Task<PagedList<Pilot>> GetAsync(PilotParameters parameters)
    {
        string cacheKey = GetListCacheKey(parameters);
        string? cachedPilots = await _cache.GetStringAsync(cacheKey);

        PagedList<Pilot> pilots;
        
        if (string.IsNullOrEmpty(cachedPilots))
        {
            pilots = await _decorated.GetAsync(parameters);
            var options = new DistributedCacheEntryOptions
            {
                SlidingExpiration = TimeSpan.FromSeconds(100),
            };

            var resolver = new IgnorePropertiesResolver();
            resolver.IgnoreProperty(typeof(Aircraft), "Pilots");

            var settings = new JsonSerializerSettings { ContractResolver = resolver };
            
            await _cache.SetStringAsync(
                cacheKey,
                JsonConvert.SerializeObject(pilots, settings),
                options
            );

            return pilots;
        }

        pilots = JsonConvert.DeserializeObject<PagedList<Pilot>>(cachedPilots);

        return pilots;
    }
    
    private string GetListCacheKey(PilotParameters parameters)
    {
        var keyParts = new List<string>
        {
            $"Age={parameters.Age?.ToString() ?? "any"}",
            $"Rating={parameters.Rating?.ToString() ?? "any"}",
            $"FirstName={parameters.FirstName ?? "any"}",
            $"LastName={parameters.LastName ?? "any"}",
            $"AircraftId={parameters.AircraftId?.ToString() ?? "any"}",
            $"PageNumber={parameters.PageNumber}",
            $"PageSize={parameters.PageSize}"
        };

        return "pilots.paged:" + string.Join("_", keyParts.Where(part => !string.IsNullOrEmpty(part)));
    }

    private async void PurgeCache(int? id=null)
    {
        ConnectionMultiplexer redis = ConnectionMultiplexer.Connect("localhost");
        var server = redis.GetServer("localhost", 6379); 


        var pageSize = 250;
        var cursor = (RedisValue) "0";

        do
        {
            var redisResult = await server.ExecuteAsync(
                "SCAN",
                cursor,
                "MATCH",
                "pilots.paged*",
                "COUNT",
                pageSize);
            
            var innerResult = (RedisResult[])redisResult;
            cursor = (RedisValue)innerResult[0];

            foreach (var key in (RedisValue[])innerResult[1])
            {
                await _cache.RemoveAsync(key);
            }
        } while (cursor != "0");
        
        if (id is not null)
        {
            await _cache.RemoveAsync(GetPilotCacheKey(id.Value));
            await _cache.RemoveAsync(GetPilotCacheKey(id.Value, true));
        }
    }
}
