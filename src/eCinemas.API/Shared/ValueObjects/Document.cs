﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace eCinemas.API.Shared.ValueObjects;

public class Document
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = default!;
}

public class TrackingDocument : Document
{
    public DateTimeOffset CreatedAt { get; set; }
    
    public string? CreatedBy { get; set; }
    
    public DateTimeOffset? ModifiedAt { get; set; }
    
    public string? ModifiedBy { get; set; }

    public virtual void MarkCreated(string? createdBy = null)
    {
        CreatedAt = DateTimeOffset.Now;
        CreatedBy = createdBy;
    }

    public virtual void MarkModified(string? modifiedBy = null)
    {
        ModifiedAt = DateTimeOffset.Now;
        ModifiedBy = modifiedBy;
    }
}