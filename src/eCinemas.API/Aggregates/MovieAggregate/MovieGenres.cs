using eCinemas.API.Shared.ValueObjects;

namespace eCinemas.API.Aggregates.MovieAggregate;

public class MovieGenres : TrackingDocument
{
    public string Name { get; set; } = default!;
}