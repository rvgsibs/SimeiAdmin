using System.Text;
using System.Text.Json;

namespace SimeiAdmin.Core.Extensions;

public static class ObjectMethods
{
    public static string ToJSON(this object obj)
    {
        return JsonSerializer.Serialize(obj);
    }

    public static StringContent ToStringContent(this object obj)
    {
        var json = JsonSerializer.Serialize(obj);
        return new StringContent(json, Encoding.UTF8, "application/json");
    }

    public static async Task<T> ToDeserializeAsync<T>(this HttpResponseMessage obj)
    {
        return JsonSerializer.Deserialize<T>
                            (await obj.Content.ReadAsStringAsync(),
                            new JsonSerializerOptions
                            {
                                PropertyNameCaseInsensitive = true
                            });
    }


}