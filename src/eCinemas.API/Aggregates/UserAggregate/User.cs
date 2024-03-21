using eCinemas.API.Shared.ValueObjects;

namespace eCinemas.API.Aggregates.UserAggregate;

public class User : TrackingDocument
{
    public string FullName { get; set; } = default!;

    public string Email { get; set; } = default!;
    
    public string? AvatarUrl { get; set; }

    public string PasswordHash { get; set; } = default!;
}