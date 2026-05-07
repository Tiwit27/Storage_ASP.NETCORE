using Storage.Enums;

namespace Storage.DTOs;

public class StorageEntryDto
{
    public string Name { get; set; } = null!;
    public StorageEntryType Type { get; set; }
    public long? Size {get; set;}
    public DateTime LastModified { get; set; }
}