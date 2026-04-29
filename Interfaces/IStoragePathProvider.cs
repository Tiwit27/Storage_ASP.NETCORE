namespace Storage.Interfaces;

public interface IStoragePathProvider
{
    string BasePath { get; }
    string GetFullPath(string? relativePath);
}