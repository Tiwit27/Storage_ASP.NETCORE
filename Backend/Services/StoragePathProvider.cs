using Microsoft.Extensions.Options;
using Storage.Exceptions;
using Storage.Interfaces;
using Storage.Options;

namespace Storage.Services;

public class StoragePathProvider : IStoragePathProvider
{
    private readonly string _basePath;

    public StoragePathProvider(IOptions<StorageOptions> options)
    {
        var rawPath = options.Value.DefaultPath ?? 
                      throw new InvalidOperationException("DefaultPath is not configured");

        _basePath = Path.GetFullPath(rawPath)
                        .TrimEnd(Path.DirectorySeparatorChar)
                    + Path.DirectorySeparatorChar;
    }

    public string BasePath => _basePath;

    public string GetFullPath(string? relativePath)
    {
        if (string.IsNullOrEmpty(relativePath))
        {
            return _basePath;
        }

        var fullPath = Path.GetFullPath(Path.Combine(_basePath, relativePath));
        var relative = Path.GetRelativePath(_basePath, fullPath);
        if (relative == ".." || relative.StartsWith(".." + Path.DirectorySeparatorChar))
        {
            throw new ForbiddenException("Forbidden path");
        }

        return fullPath;
    }
}