using Storage.DTOs;
using Storage.Enums;
using Storage.Exceptions;
using Storage.Interfaces;
using FileNotFoundException = System.IO.FileNotFoundException;

namespace Storage.Services;

public class StorageService
{
    private readonly IStoragePathProvider _pathProvider;
    public StorageService(IStoragePathProvider pathProvider)
    {
        _pathProvider = pathProvider;
    }
    
    public IEnumerable<StorageEntryDto> GetEntries(string? path)
    {
        var fullPath = _pathProvider.GetFullPath(path);
        if (!Directory.Exists(fullPath))
        {
            throw new FileNotFoundException("Directory not found");
        }
        var entries = new DirectoryInfo(fullPath).EnumerateFileSystemInfos();
        return new DirectoryInfo(fullPath)
            .EnumerateFileSystemInfos()
            .Select(entry => new StorageEntryDto
            {
                Name = entry.Name,
                Type = entry is DirectoryInfo ? StorageEntryType.Directory : StorageEntryType.File,
                Size = entry is FileInfo file ? file.Length : null,
                LastModified = entry.LastWriteTime
            });
    }

    public async Task<string> UploadFile(IFormFile file, string? path)
    {
        try
        {
            if (file.Length > 20971520) //20MB max
            {
                Console.WriteLine("Error");
                throw new FileSizeLimitException("File is too large");
            }
            var fullPath = _pathProvider.GetFullPath(path);
            if (!Directory.Exists(fullPath))
            {
                throw new FileNotFoundException("Directory not found");
            }
            fullPath = Path.Combine(fullPath,Path.GetFileName(file.FileName));
            await using var stream = new FileStream(fullPath, FileMode.Create);
            await file.CopyToAsync(stream);
            return fullPath;
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }
}