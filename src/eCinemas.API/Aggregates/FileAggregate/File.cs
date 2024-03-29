using eCinemas.API.Shared.ValueObjects;

namespace eCinemas.API.Aggregates.FileAggregate;

public class File : ModifierTrackingDocument
{
    public string SourceName { get; set; } = default!;

    public string FileName { get; set; } = default!;

    public string Path { get; set; } = default!;

    public string ContentType { get; set; } = default!;
    
    public long Size { get; set; }
}