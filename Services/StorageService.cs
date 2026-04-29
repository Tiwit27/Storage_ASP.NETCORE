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

    public async Task<bool> UploadFile(IFormFile file, string? path)
    {
        var fullPath = _pathProvider.GetFullPath(path);
        if (!Directory.Exists(fullPath))
        {
            throw new FileNotFoundException("Directory not found");
        }
        var safeName = Path.GetFileName(file.FileName);
        using var stream = new FileStream(Path.Combine(fullPath, safeName), FileMode.Create);
        await file.CopyToAsync(stream);
        return true;
    }
}