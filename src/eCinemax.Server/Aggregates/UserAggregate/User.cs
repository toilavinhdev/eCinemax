using eCinemax.Server.Shared.ValueObjects;

namespace eCinemax.Server.Aggregates.UserAggregate;

public class User : TimeTrackingDocument
{
    public string FullName { get; set; } = default!;

    public string Email { get; set; } = default!;
    
    public string? AvatarUrl { get; set; }

    public string PasswordHash { get; set; } = default!;
}