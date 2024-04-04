namespace eCinemax.Server.Shared.BackgroundJob;

public class HangfireConfig
{
    public string Title { get; set; } = default!;

    public string Database { get; set; } = default!;

    public HangfireAuthentication Authentication { get; set; } = default!;
}

public class HangfireAuthentication
{
    public string UserName { get; set; } = default!;

    public string Password { get; set; } = default!;
}