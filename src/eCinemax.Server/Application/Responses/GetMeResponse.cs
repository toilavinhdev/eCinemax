namespace eCinemax.Server.Application.Responses;

public class GetMeResponse
{
    public string Id { get; set; } = default!;
    
    public string FullName { get; set; } = default!;

    public string Email { get; set; } = default!;
    
    public string? AvatarUrl { get; set; }
}