namespace eCinemas.API.ValueObjects;

public class AppSettings
{
    public string Host { get; set; } = default!;

    public StaticFileConfig StaticFileConfig { get; set; } = default!;
}

public class StaticFileConfig
{
    public string Location { get; set; } = default!;
    
    public string? External { get; set; }
}