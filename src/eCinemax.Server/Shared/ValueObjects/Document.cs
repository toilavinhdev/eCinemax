using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace eCinemax.Server.Shared.ValueObjects;

public interface IAggregateRoot;

public class Document
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = default!;
}

public class TimeTrackingDocument : Document
{
    [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
    public DateTime CreatedAt { get; set; }
    
    [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
    public DateTime ModifiedAt { get; set; }
    
    public virtual void MarkCreated()
    {
        CreatedAt = DateTime.Now;
        ModifiedAt = DateTime.Now;
    }

    public virtual void MarkModified()
    {
        ModifiedAt = DateTime.Now;
    }
}

public class ModifierTrackingDocument : TimeTrackingDocument
{
    [BsonRepresentation(BsonType.ObjectId)]
    public string CreatedBy { get; set; } = default!;

    [BsonRepresentation(BsonType.ObjectId)]
    public string ModifiedBy { get; set; } = default!;

    public virtual void MarkCreated(string createdBy)
    {
        CreatedAt = DateTime.Now;
        CreatedBy = createdBy;
        ModifiedBy = createdBy;
    }

    public virtual void MarkModified(string modifiedBy)
    {
        ModifiedAt = DateTime.Now;
        ModifiedBy = modifiedBy;
    }
}