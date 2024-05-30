using Domain.DTO;

namespace Services.IRepositories;

public interface IAuthenticationService
{
    Task<string> GetCurrentTokenFromLocalStorage();
    Task<ResponseDto> RegisterUser(UserRegistrationDTO userForRegistration);
    Task<TokenDTO> Login(UserAuthenticationDTO userForAuthentication);
    Task Logout();
    Task<string> RefreshToken();
}