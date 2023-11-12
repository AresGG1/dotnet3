using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace DAL.Converters;

public class IgnorePropertiesResolver : DefaultContractResolver
{
    private readonly Dictionary<Type, HashSet<string>> _ignores = new();
    
    public void IgnoreProperty(Type type, params string[] jsonPropertyNames)
    {
        if (!_ignores.ContainsKey(type))
        {
            _ignores[type] = new HashSet<string>();
        }
    
        foreach (var prop in jsonPropertyNames)
        {
            _ignores[type].Add(prop);
        }
    }
    
    protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
    {
        JsonProperty property = base.CreateProperty(member, memberSerialization);
    
        if (_ignores.TryGetValue(member.DeclaringType, out var properties) 
            && properties.Contains(property.PropertyName))
        {
            property.ShouldSerialize = _ => false;
        }
    
        return property;
    }
}
