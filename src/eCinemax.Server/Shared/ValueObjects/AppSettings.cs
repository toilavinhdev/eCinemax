using eCinemax.Server.Shared.Library.VnPay;

namespace eCinemax.Server.Shared.ValueObjects;

public class AppSettings
{
    public string Host { get; set; } = default!;
    
    public StaticFileConfig StaticFileConfig { get; set; } = default!;

    public MongoConfig MongoConfig { get; set; } = default!;

    public JwtConfig JwtConfig { get; set; } = default!;

    public GoogleOAuthConfig GoogleOAuthConfig { get; set; } = default!;

    public GmailConfig GmailConfig { get; set; } = default!;

    public HangfireConfig HangfireConfig { get; set; } = default!;

    public VnPayConfig VnPayConfig { get; set; } = default!;
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

public class GoogleOAuthConfig
{
    public string ClientId { get; set; } = default!;

    public string ClientSecret { get; set; } = default!;
}

public class GmailConfig
{
    public string Host { get; set; } = default!;
    
    public int Port { get; set; }
    
    public string DisplayName { get; set; } = default!;
    
    public string Mail { get; set; } = default!;
    
    public string Password { get; set; } = default!;
}

public class HangfireConfig
{
    public string Title { get; set; } = default!;
    
    public string Path { get; set; } = default!;

    public string UserName { get; set; } = default!;

    public string Password { get; set; } = default!;
}