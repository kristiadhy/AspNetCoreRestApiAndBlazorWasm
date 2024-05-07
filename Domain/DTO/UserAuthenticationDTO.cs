using System.ComponentModel.DataAnnotations;

namespace Domain.DTO;
public record UserAuthenticationDTO
{
    [Required(ErrorMessage = "User name is required")]
    public string? UserName { get; init; }
    [Required(ErrorMessage = "Password name is required")]
    public string? Password { get; init; }
}
