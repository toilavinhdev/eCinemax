namespace eCinemas.API.Shared.ValueObjects;

public class AppSettings
{
    public string Host { get; set; } = default!;

    public StaticFileConfig StaticFileConfig { get; set; } = default!;

    public MongoConfig MongoConfig { get; set; } = default!;

    public JwtConfig JwtConfig { get; set; } = default!;
}

public class StaticFileConfig
{
    public string Location { get; set; } = default!;
    
    public string? External { get; set; }
}

public class MongoConfig
{
    public string ConnectionString { get; set; } = default!;

    public string DatabaseName { get; set; } = default!;
}

public class JwtConfig
{
    public string TokenSingingKey { get; set; } = default!;
    
    public int AccessTokenDurationInMinutes { get; set; }
}