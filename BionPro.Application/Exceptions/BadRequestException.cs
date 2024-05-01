namespace Application.Exceptions;

public abstract class BadRequestException(string message) : Exception(message)
{
}