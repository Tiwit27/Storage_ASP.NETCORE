namespace Storage.Exceptions;

public class FileSizeLimitException : Exception
{
    public FileSizeLimitException(string message) : base("File Size Limit")
    {
        
    }
}