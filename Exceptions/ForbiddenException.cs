namespace Storage.Exceptions;

public class ForbiddenException : Exception
{
    public ForbiddenException(string message) : base("Forbidden")
    {
        
    }
}