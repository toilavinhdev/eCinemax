namespace eCinemax.Server.Shared.Extensions;

public static class StringExtensions
{
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