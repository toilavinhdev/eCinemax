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
}