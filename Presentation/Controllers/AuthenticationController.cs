using Domain.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.ActionFilters;
using Services.Contracts;

namespace Presentation.Controllers;

[ApiVersion("1.0")]
[ApiController]
[Route("api/authentication")]
public class AuthenticationController(IServiceManager serviceManager) : ControllerBase
{
    private readonly IServiceManager _serviceManager = serviceManager;

    [HttpPost, AllowAnonymous]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    public async Task<IActionResult> RegisterUser([FromBody] UserRegistrationDTO userForRegistration)
    {
        //Look into identityresult class from asp.net core identity for more details.
        var result = await _serviceManager.AuthenticationService.RegisterUser(userForRegistration);
        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
                ModelState.TryAddModelError(error.Code, error.Description);

            return BadRequest(ModelState);
        }

        return Created();
    }

    [HttpPost("login"), AllowAnonymous]
    [ServiceFilter(typeof(ValidationFilterAttribute))] 
    public async Task<IActionResult> Authenticate([FromBody] UserAuthenticationDTO user) 
    { 
        if (!await _serviceManager.AuthenticationService.ValidateUser(user)) 
            return Unauthorized(); 
        return Ok(new { 
            Token = await _serviceManager.AuthenticationService.CreateToken() 
        }); 
    }
}
