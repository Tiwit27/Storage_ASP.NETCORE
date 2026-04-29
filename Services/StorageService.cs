using Storage.Exceptions;
using FileNotFoundException = System.IO.FileNotFoundException;

namespace Storage.Services;

public class StorageService
{
    private readonly string _defaultPath;
    public StorageService(IConfiguration configuration)
    {
        _defaultPath = configuration.GetValue<string>("DefaultPath") ??
                       throw new InvalidOperationException("DefaultPath is not configured");
    }
    
    public IEnumerable<string?> GetEntries(string? path)
    {
        var fullPath = "";
        if (path == null)
        {
            fullPath = Path.GetFullPath(_defaultPath);
        }
        else
        {
            var basePath = Path.GetFullPath(_defaultPath).TrimEnd(Path.DirectorySeparatorChar) + Path.DirectorySeparatorChar;
            fullPath = Path.GetFullPath(Path.Combine(basePath, path));
            var relative = Path.GetRelativePath(basePath, fullPath);
            if (relative == ".." || relative.StartsWith(".." + Path.DirectorySeparatorChar))
            {
                throw new ForbiddenException("Forbidden path");
            }
        }
        if (!Directory.Exists(fullPath))
        {
            throw new FileNotFoundException("Directory not found");
        }
        return Directory.EnumerateFileSystemEntries(fullPath).Select(Path.GetFileName);
    }
    
    public IEnumerable<string?> GetFiles(string? path)
    {
        var fullPath = "";
        if (path == null)
        {
            fullPath = Path.GetFullPath(_defaultPath);
        }
        else
        {
            var basePath = Path.GetFullPath(_defaultPath).TrimEnd(Path.DirectorySeparatorChar) + Path.DirectorySeparatorChar;
            fullPath = Path.GetFullPath(Path.Combine(basePath, path));
            if (!fullPath.StartsWith(basePath, StringComparison.OrdinalIgnoreCase))
            {
                throw new ForbiddenException("Forbidden path");
            }
        }
        if (!Directory.Exists(fullPath))
        {
            throw new FileNotFoundException("Directory not found");
        }
        return Directory.EnumerateFileSystemEntries(fullPath).Select(Path.GetFileName);
    }
}