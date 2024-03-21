using System.Security.Cryptography;
using System.Text;

namespace eCinemas.API.Shared.Extensions;

public static class StringExtensions
{
    public static string ToSha256(this string input)
    {
        if (string.IsNullOrEmpty(input)) return default!;
        var hashed = SHA256.HashData(Encoding.UTF8.GetBytes(input));
        var stringBuilder = new StringBuilder();
        foreach (var byteCode in hashed)
            stringBuilder.Append(byteCode.ToString("X2"));
        return stringBuilder.ToString();
    }
}