namespace auth_service.Exceptions.Common;

public class BadRequestException : Exception
{
    public BadRequestException(string message)
        : base(message)
    {
    }
}