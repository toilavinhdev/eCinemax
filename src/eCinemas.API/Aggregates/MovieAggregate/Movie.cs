﻿using eCinemas.API.Shared.ValueObjects;

namespace eCinemas.API.Aggregates.MovieAggregate;

public class Movie : TrackingDocument 
{
    public string Title { get; set; } = default!;
    
    public string? Description { get; set; }
    
    public MovieStatus Status { get; set; }
    
    public List<string>? Genres { get; set; }
    
    public List<string>? ImageUrls { get; set; }
    
    public long DurationMinutes { get; set; }
}

public enum MovieStatus
{
    ComingSoon = 0,
    NowShowing,
    StopShowing
}