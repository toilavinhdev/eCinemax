using System.Diagnostics.CodeAnalysis;

namespace eCinemas.API.Shared.Exceptions;

public class DocumentNotFoundException<T> : BadRequestException
{
    public DocumentNotFoundException(string message) : base(message)
    { }

    public DocumentNotFoundException(string message, Exception inner) : base(message, inner)
    { }

    public static void ThrowIfNotFound([NotNull] object? value, string? parameter = null)
    {
        if (value is not null) return;
        var message = $"Không tìm thấy bản ghi ${nameof(T)} ${parameter}";
        throw new DocumentNotFoundException<T>(message);
    }
}