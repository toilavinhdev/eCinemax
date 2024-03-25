namespace eCinemas.API.Shared.Extensions;

public static class EnumerableExtensions
{
    public static IEnumerable<(T item, int index)> WithIndexes<T>(this IEnumerable<T> input)
        => input.Select((item, idx) => (item, idx));
}