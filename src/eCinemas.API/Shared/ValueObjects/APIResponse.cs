using System.Text.Json.Serialization;

namespace eCinemas.API.Shared.ValueObjects;

public class APIResponse
{
    public bool Success { get; set; }
    
    public int Code { get; set; }
    
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Message { get; set; }
    
    public static APIResponse IsSuccess(string? message = null)
        => new()
        {
            Success = true,
            Code = 200,
            Message = message
        };
}
public class APIResponse<T> : APIResponse
{
    public T Data { get; set; } = default!;
    
    public APIResponse<T> IsSuccess(T data, string? message = null) 
        => new()
        {
            Success = true,
            Code = 200,
            Message = message,
            Data = data
        };
}