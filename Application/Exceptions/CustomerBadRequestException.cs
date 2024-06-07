namespace Application.Exceptions;

public sealed class IdParametersBadRequestException : BadRequestException
{
    public IdParametersBadRequestException() : base("Customer is null") { }
}
