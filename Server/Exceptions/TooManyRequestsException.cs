namespace Stocks.Server.Exceptions;

public class TooManyRequestsException : Exception
{
    public TooManyRequestsException(string? message) : base(message)
    {
    }

    public TooManyRequestsException() : base("Too many requests made")
    {
        
    }
}