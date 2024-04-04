using System.Diagnostics.CodeAnalysis;

namespace eCinemax.Server.Shared.Exceptions;

public class DocumentNotFoundException<T> : BadRequestException
{
    public DocumentNotFoundException(string message) : base(message) { }

    public DocumentNotFoundException(string message, Exception inner) : base(message, inner) { }

    public static void ThrowIfNotFound([NotNull] object? value, string? message = null)
    {
        if (value is not null) return;
        throw new DocumentNotFoundException<T>(message ?? string.Empty);
    }
}