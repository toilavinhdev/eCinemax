using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace eCinemas.API.Shared.ValueObjects;

public class Document
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = default!;
}

public class TimeTrackingDocument : Document
{
    public DateTimeOffset CreatedAt { get; set; }
    
    public DateTimeOffset ModifiedAt { get; set; }
    
    public virtual void MarkCreated()
    {
        CreatedAt = DateTimeOffset.UtcNow;
        ModifiedAt = DateTimeOffset.Now;
    }

    public virtual void MarkModified()
    {
        ModifiedAt = DateTimeOffset.UtcNow;
    }
}

public class ModifierTrackingDocument : TimeTrackingDocument
{
    public string CreatedBy { get; set; } = default!;

    public string ModifiedBy { get; set; } = default!;

    public virtual void MarkCreated(string createdBy)
    {
        CreatedAt = DateTimeOffset.UtcNow;
        CreatedBy = createdBy;
        ModifiedBy = createdBy;
    }

    public virtual void MarkModified(string modifiedBy)
    {
        ModifiedAt = DateTimeOffset.UtcNow;
        ModifiedBy = modifiedBy;
    }
}