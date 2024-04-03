namespace eCinemas.API.Shared.Library.VnPay;

public class VnPayConfig
{
    public string Version { get; set; } = default!;

    public string Url { get; set; } = default!;

    public string ReturnEndpoint { get; set; } = default!;

    public string TmnCode { get; set; } = default!;

    public string HashSecret { get; set; } = default!;
}