﻿using eCinemax.Server.Shared.ValueObjects;

namespace eCinemax.Server.Aggregates.FileAggregate;

public class File : ModifierTrackingDocument, IAggregateRoot
{
    public string SourceName { get; set; } = default!;

    public string FileName { get; set; } = default!;

    public string Path { get; set; } = default!;

    public string ContentType { get; set; } = default!;
    
    public long Size { get; set; }
}