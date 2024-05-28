namespace Application.Exceptions;
public sealed class RefreshTokenBadRequest : BadRequestException
{
    public RefreshTokenBadRequest() : base("Invalid client request. The token has some invalid values.") { }
}
