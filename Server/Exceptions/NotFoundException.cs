namespace Stocks.Server.Exceptions;

public class NotFoundException : Exception
{

    public NotFoundException(string message) : base($"{message} not found")
    {
    }
}