using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Net;
using System.Text;
using Todo.NET.Extensions;

namespace eCinemax.Server.Shared.Library.VnPay;

[SuppressMessage("ReSharper", "InconsistentNaming")]
public class VnPayRequest
{
    private SortedList<string, string> _params = new(new VnPayCompare());

    public string vnp_Version { get; set; } = default!;

    public string vnp_Command { get; set; } = default!;

    public string vnp_TmnCode { get; set; } = default!;

    public int vnp_Amount { get; set; }

    public string? vnp_BankCode { get; set; }

    public string vnp_CreateDate { get; set; } = default!;

    public string vnp_CurrCode { get; set; } = default!;

    public string vnp_IpAddr { get; set; } = default!;

    public string vnp_Locale { get; set; } = default!;

    public string vnp_OrderInfo { get; set; } = default!;

    public string vnp_OrderType { get; set; } = default!;

    public string vnp_ReturnUrl { get; set; } = default!;

    public string vnp_TxnRef { get; set; } = default!;

    private string vnp_SecureHash { get; set; } = default!;
    
    public string CreateRequestUrl(string baseUrl, string hashSecret)
    {
        MakeParams();
        var stringBuilder = new StringBuilder();
        foreach (var pair in _params.Where(pair => !string.IsNullOrEmpty(pair.Value)))
            stringBuilder.Append(WebUtility.UrlEncode(pair.Key) + "=" + WebUtility.UrlEncode(pair.Value) + "&");

        baseUrl += "?" + stringBuilder;
        
        vnp_SecureHash = stringBuilder
            .ToString()
            .Remove(stringBuilder.Length - 1, 1) //remove last '&'
            .ToHmacSha512(hashSecret);
        
        baseUrl += nameof(vnp_SecureHash) + "=" + vnp_SecureHash;
        
        return baseUrl;
    }

    private void MakeParams()
    {
        AddParam(nameof(vnp_Version), vnp_Version);
        AddParam(nameof(vnp_Command), vnp_Command);
        AddParam(nameof(vnp_TmnCode), vnp_TmnCode);
        AddParam(nameof(vnp_Amount), vnp_Amount.ToString(CultureInfo.CurrentCulture));
        AddParam(nameof(vnp_BankCode), vnp_BankCode);
        AddParam(nameof(vnp_CreateDate), vnp_CreateDate);
        AddParam(nameof(vnp_CurrCode), vnp_CurrCode);
        AddParam(nameof(vnp_IpAddr), vnp_IpAddr);
        AddParam(nameof(vnp_Locale), vnp_Locale);
        AddParam(nameof(vnp_OrderInfo), vnp_OrderInfo);
        AddParam(nameof(vnp_OrderType), vnp_OrderType);
        AddParam(nameof(vnp_ReturnUrl), vnp_ReturnUrl);
        AddParam(nameof(vnp_TxnRef), vnp_TxnRef);
        AddParam(nameof(vnp_SecureHash), vnp_SecureHash);
    }
    
    private void AddParam(string key, string? value)
    {
        if (string.IsNullOrEmpty(value)) return;
        _params.Add(key, value);
    }
}