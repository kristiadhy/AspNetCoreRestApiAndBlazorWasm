using System.ComponentModel.DataAnnotations;

namespace Domain.DTO;
public record UserAuthenticationDTO
{
    public UserAuthenticationDTO(string userName, string password)
    {
        UserName = userName;
        Password = password;
    }

    [Required(ErrorMessage = "User name is required")]
    public string? UserName { get; init; }
    [Required(ErrorMessage = "Password name is required")]
    public string? Password { get; init; }
}
