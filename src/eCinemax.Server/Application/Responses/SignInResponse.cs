namespace eCinemax.Server.Application.Responses;

public class SignInResponse
{
    public string AccessToken { get; set; } = default!;

    public string RefreshToken { get; set; } = default!;
}