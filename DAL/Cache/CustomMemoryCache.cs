using Microsoft.Extensions.Caching.Memory;

namespace DAL.Cache;

public class CustomMemoryCache
{
    public MemoryCache Cache { get; } = new MemoryCache(
        new MemoryCacheOptions
        {
            SizeLimit = 1024
        }); 
    private readonly List<string> _keys = new List<string>();
    
    public void AddKey(string key)
    {
        _keys.Add(key);
    }

    public List<string> GetParameterKeys()
    {
        return _keys.FindAll(k => k.StartsWith("aircrafts.paged:"));
    }
}
