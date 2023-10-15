using System.Text.Json;
using DAL.Pagination;

namespace lab3.Extensions;

public static class PagedListExtensions
{
    public static string SerializeMetadata<T>(this PagedList<T> list)
    {
        return JsonSerializer.Serialize(
            list.Metadata,
            new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
    }
}