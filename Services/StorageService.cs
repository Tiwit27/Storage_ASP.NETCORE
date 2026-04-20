using Storage.Exceptions;
using FileNotFoundException = System.IO.FileNotFoundException;

namespace Storage.Services;

public class StorageService
{
    private string _defaultPath;
    public StorageService(IConfiguration configuration)
    {
        _defaultPath = configuration.GetValue<string>("DefaultPath");
    }
    
    public IEnumerable<string>? GetFiles(string? path)
    {
        var fullPath = "";
        if (path == null)
        {
            fullPath = Path.GetFullPath(_defaultPath);
        }
        else
        {
            var basePath = Path.GetFullPath(_defaultPath);
            fullPath = Path.GetFullPath(Path.Join(basePath, path));
            basePath += Path.DirectorySeparatorChar;
            Console.WriteLine(basePath);
            if (!fullPath.StartsWith(basePath))
            {
                throw new ForbiddenException("Forbidden path");
            }
        }
        if (!Directory.Exists(fullPath))
        {
            throw new FileNotFoundException("Directory not found");
        }
        string[] files = Directory.GetFileSystemEntries(fullPath);
        return files;
    }
}