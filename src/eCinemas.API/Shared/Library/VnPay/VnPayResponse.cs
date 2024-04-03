using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Text;
using eCinemas.API.Shared.Extensions;

namespace eCinemas.API.Shared.Library.VnPay;

[SuppressMessage("ReSharper", "InconsistentNaming")]
public class VnPayResponse
{
    private SortedList<string, string> _params = new(new VnPayCompare());

    public string vnp_TmnCode { get; set; } = default!;

    public string vnp_Amount { get; set; } = default!;

    public string vnp_BankCode { get; set; } = default!;

    public string vnp_BankTranNo { get; set; } = default!;

    public string vnp_CardType { get; set; } = default!;

    public string vnp_PayDate { get; set; } = default!;

    public string vnp_OrderInfo { get; set; } = default!;

    public string vnp_TransactionNo { get; set; } = default!;

    public string vnp_ResponseCode { get; set; } = default!;

    public string vnp_TransactionStatus { get; set; } = default!;

    public string vnp_TxnRef { get; set; } = default!;

    public string? vnp_SecureHashType { get; set; }

    public string vnp_SecureHash { get; set; } = default!;

    public bool IsValidSignature(string hashSecret)
    {
        MakeParams();
        var stringBuilder = new StringBuilder();
        foreach (var pair in _params.Where(pair => !string.IsNullOrEmpty(pair.Value)))
            stringBuilder.Append(WebUtility.UrlEncode(pair.Key) + "=" + WebUtility.UrlEncode(pair.Value) + "&");

        var checkSum = stringBuilder
            .ToString()
            .Remove(stringBuilder.Length - 1, 1) //remove last '&'
            .ToHmacSha512(hashSecret);

        return checkSum.Equals(vnp_SecureHash, StringComparison.InvariantCultureIgnoreCase);
    }

    private void MakeParams()
    {
        AddParam(nameof(vnp_TmnCode), vnp_TmnCode);
        AddParam(nameof(vnp_Amount), vnp_Amount);
        AddParam(nameof(vnp_BankCode), vnp_BankCode);
        AddParam(nameof(vnp_BankTranNo), vnp_BankTranNo);
        AddParam(nameof(vnp_CardType), vnp_CardType);
        AddParam(nameof(vnp_PayDate), vnp_PayDate);
        AddParam(nameof(vnp_OrderInfo), vnp_OrderInfo);
        AddParam(nameof(vnp_TransactionNo), vnp_TransactionNo);
        AddParam(nameof(vnp_ResponseCode), vnp_ResponseCode);
        AddParam(nameof(vnp_TransactionStatus), vnp_TransactionStatus);
        AddParam(nameof(vnp_TxnRef), vnp_TxnRef);
        AddParam(nameof(vnp_SecureHashType), vnp_SecureHashType);
    }

    private void AddParam(string key, string? value)
    {
        if (string.IsNullOrEmpty(value)) return;
        _params.Add(key, value);
    }
}