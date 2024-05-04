using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO;
public record UserAuthenticationDTO
{
    [Required(ErrorMessage = "User name is required")] 
    public string? UserName { get; init; }
    [Required(ErrorMessage = "Password name is required")] 
    public string? Password { get; init; }
}
