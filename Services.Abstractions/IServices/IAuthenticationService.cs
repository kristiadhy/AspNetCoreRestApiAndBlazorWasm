using Domain.DTO;
using Microsoft.AspNetCore.Identity;

namespace Services.Contracts;
public interface IAuthenticationService
{
    Task<IdentityResult> RegisterUser(UserRegistrationDTO userForRegistration);
    Task<bool> ValidateUser(UserAuthenticationDTO userForAuth);
    Task<TokenDTO> CreateToken(bool populateExp);
    Task<TokenDTO> RefreshToken(TokenDTO tokenDto);
}
