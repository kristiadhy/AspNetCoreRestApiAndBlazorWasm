using Application.Repositories;
using Application.Validators;
using AutoMapper;
using Domain.DTO;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Services.Contracts;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Services.Services;
public class AuthenticationService : IAuthenticationService
{
    private readonly IRepositoryManager _repositoryManager;
    private readonly IMapper _mapper;
    private readonly ILogger _logger;
    private readonly UserManager<UserMD> _userManager; //This class is used to provide the APIs for managing users in a persistence store
    private readonly IConfiguration _configuration;
    
    private UserMD? _user;

    public AuthenticationService(IRepositoryManager repositoryManager, IMapper mapper, ILogger logger, UserManager<UserMD> userManager, IConfiguration configuration)
    {
        _repositoryManager = repositoryManager;
        _mapper = mapper;
        _logger = logger;
        _userManager = userManager;
        _configuration = configuration;
    }

    public async Task<IdentityResult> RegisterUser(UserRegistrationDTO userForRegistration)
    {
        var validator = new UserDtoValidator();
        validator.ValidateInput(userForRegistration);

        var user = _mapper.Map<UserMD>(userForRegistration);
        var result = await _userManager.CreateAsync(user, userForRegistration.Password!);
        if (result.Succeeded)
            await _userManager.AddToRolesAsync(user, userForRegistration.Roles!);

        return result;
    }

    public async Task<bool> ValidateUser(UserAuthenticationDTO userForAuth)
    {
        _user = await _userManager.FindByNameAsync(userForAuth.UserName!);
        var result = (_user != null && await _userManager.CheckPasswordAsync(_user, userForAuth.Password!));
        if (!result)
            _logger.Warning($"{nameof(ValidateUser)}: Authentication failed. Wrong user name or password.");

        return result;
    }

    public async Task<string> CreateToken()
    {
        var signingCredentials = GetSigningCredentials();
        var claims = await GetClaims();
        var tokenOptions = GenerateTokenOptions(signingCredentials, claims);
        return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
    }

    private SigningCredentials GetSigningCredentials()
    {
        var key = Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")!);
        var secret = new SymmetricSecurityKey(key); 
        return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
    }

    private async Task<List<Claim>> GetClaims()
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, _user!.UserName!)
        };
        var roles = await _userManager.GetRolesAsync(_user);
        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }
        return claims;
    }

    private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims) 
    { 
        var jwtSettings = _configuration.GetSection("JwtSettings"); 
        var tokenOptions = new JwtSecurityToken(
            issuer: jwtSettings["validIssuer"], 
            audience: jwtSettings["validAudience"], 
            claims: claims, 
            expires: DateTime.Now.AddMinutes(Convert.ToDouble(jwtSettings["expires"])), 
            signingCredentials: signingCredentials
            ); 
        return tokenOptions; 
    }
}
