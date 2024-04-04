using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace eCinemax.Server.Shared.Extensions;

public static class StringExtensions
{
    public static long ToLong(this string input)
    {
        var parser = long.TryParse(input, out var result);
        return parser ? result : 0;
    }
    
    public static string JoinString(this IEnumerable<string> input, string separator)
    {
        return string.Join(separator, input);
    }
    
    public static string ToQueryString(this object input)
    {
        var query = input.GetType()
            .GetProperties()
            .Where(x => x.GetValue(input) is not null)
            .Select(x => x.Name + "=" + WebUtility.UrlEncode(x.GetValue(input)?.ToString()))
            .JoinString("&");
        return query;
    }
    
    private const string Alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

    public static int ToSeatIndex(this char input)
    {
        if (char.IsWhiteSpace(input)) return default;
        var index = Alphabet.IndexOf(input);
        if (index == -1) 
            throw new ArgumentException("The input contains a character that is not a valid alphabet letter.");
        return index;
    }

    public static char ToSeatCharacter(this int index)
    {
        if (index < 0) throw new ArgumentException("The input must greater than 0");
        return Alphabet[index];
    }
    
    public static string ToSha256(this string input)
    {
        if (string.IsNullOrEmpty(input)) return default!;
        var hashed = SHA256.HashData(Encoding.UTF8.GetBytes(input));
        var stringBuilder = new StringBuilder();
        foreach (var byteCode in hashed)
            stringBuilder.Append(byteCode.ToString("X2"));
        return stringBuilder.ToString();
    }

    public static string ToHmacSha512(this string input, string secret)
    {
        if (string.IsNullOrEmpty(input)) return default!;
        var hashed = HMACSHA512.HashData(
            Encoding.UTF8.GetBytes(secret), 
            Encoding.UTF8.GetBytes(input));
        var stringBuilder = new StringBuilder();
        foreach (var byteCode in hashed) 
            stringBuilder.Append(byteCode.ToString("X2"));
        return stringBuilder.ToString();
    }
}