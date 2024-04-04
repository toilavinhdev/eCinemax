using Newtonsoft.Json;

namespace eCinemax.Server.Shared.Extensions;

public static class JsonExtensions
{
    public static string ToJson<T>(this T input)
    {
        return JsonConvert.SerializeObject(
            input,
            new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
    }

    public static T ToObject<T>(this string json)
    {
        return JsonConvert.DeserializeObject<T>(
            json,
            new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore })!;
    }
}