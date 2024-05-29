using Domain.DTO;

namespace Services.IRepositories;

public interface IAuthenticationService
{
    Task<string> GetCurrentTokenFromLocalStorage();
    Task<RegistrationResponseDto> RegisterUser(UserRegistrationDTO userForRegistration);
    Task<TokenDTO> Login(UserAuthenticationDTO userForAuthentication);
    Task Logout();
    Task<string> RefreshToken();
}