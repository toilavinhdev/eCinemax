using System.Text.RegularExpressions;

namespace eCinemas.API.Shared.Constants;

public static class RegexConstant
{
    public static readonly Regex EmailRegex = new(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$");
}