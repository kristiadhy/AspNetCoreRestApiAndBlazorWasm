using Application.Exceptions;
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
using System.Security.Cryptography;
using System.Text;

namespace Services.Services;
public class AuthenticationService : IAuthenticationService
{
    private readonly IRepositoryManager _repositoryManager;
    private readonly IMapper _mapper;
    private readonly ILogger _logger;
    private readonly UserManager<UserMD> _userManager; //This class is use to provide the APIs for managing users in a persistence store
    private readonly IConfiguration _configuration;

    private UserMD? _user;
    private readonly JwtConfiguration _jwtConfiguration;

    public AuthenticationService(IRepositoryManager repositoryManager, IMapper mapper, ILogger logger, UserManager<UserMD> userManager, IConfiguration configuration)
    {
        _repositoryManager = repositoryManager;
        _mapper = mapper;
        _logger = logger;
        _userManager = userManager;
        _configuration = configuration;

        //We want to use JWT configuration model instead of taking it from the appsettings.json.
        //Please take a look at the JwtInstaller class for more details.
        _jwtConfiguration = new JwtConfiguration();
        _configuration.Bind(_jwtConfiguration.Section, _jwtConfiguration);
    }

    public async Task<IdentityResult> RegisterUser(UserRegistrationDTO userForRegistration)
    {
        var validator = new UserDtoValidator();
        validator.ValidateInput(userForRegistration);

        var user = _mapper.Map<UserMD>(userForRegistration);
        //The create async method and add to roles async method are the built in method provided by the microsoft identity class
        var result = await _userManager.CreateAsync(user, userForRegistration.Password!);
        if (result.Succeeded)
            await _userManager.AddToRolesAsync(user, userForRegistration.Roles!);

        return result;
    }

    public async Task<bool> ValidateUser(UserAuthenticationDTO userForAuth)
    {
        _user = await _userManager.FindByNameAsync(userForAuth.UserName!);
        var resultStatus = (_user != null && await _userManager.CheckPasswordAsync(_user, userForAuth.Password!));
        if (!resultStatus)
            _logger.Warning($"{nameof(ValidateUser)}: Authentication failed. Wrong user name or password.");

        return resultStatus;
    }

    public async Task<TokenDTO> CreateToken(bool populateExp)
    {
        var signingCredentials = GetSigningCredentials();
        var claims = await GetClaims();
        var tokenOptions = GenerateTokenOptions(signingCredentials, claims);
        var refreshToken = GenerateRefreshToken();
        _user!.RefreshToken = refreshToken;

        if (populateExp)
            _user.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);

        //Update user data in the database
        await _userManager.UpdateAsync(_user);
        var accessToken = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

        return new TokenDTO(accessToken, refreshToken);
    }

    public async Task<TokenDTO> RefreshToken(TokenDTO tokenDto)
    {
        var principal = GetPrincipalFromExpiredToken(tokenDto.AccessToken);
        var user = await _userManager.FindByNameAsync(principal.Identity!.Name!);
        if (user == null || user.RefreshToken != tokenDto.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
            throw new RefreshTokenBadRequest();

        _user = user;
        return await CreateToken(populateExp: false);
    }

    private SigningCredentials GetSigningCredentials()
    {
        var secretKey = Environment.GetEnvironmentVariable("SECRET");
        var key = Encoding.UTF8.GetBytes(secretKey!);
        var secret = new SymmetricSecurityKey(key);

        return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
    }

    private async Task<List<Claim>> GetClaims()
    {
        var lstClaims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, _user!.UserName!)
        };

        var roles = await _userManager.GetRolesAsync(_user);
        foreach (var role in roles)
        {
            lstClaims.Add(new Claim(ClaimTypes.Role, role));
        }
        return lstClaims;
    }

    private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
    {
        var tokenOptions = new JwtSecurityToken(
            issuer: _jwtConfiguration.ValidIssuer,
            audience: _jwtConfiguration.ValidAudience,
            claims: claims,
            expires: DateTime.Now.AddMinutes(Convert.ToDouble(_jwtConfiguration.Expires)),
            signingCredentials: signingCredentials
            );
        return tokenOptions;
    }

    private string GenerateRefreshToken()
    {
        //We use the RandomNumberGenerator class to generate a cryptographic random number for this purpose.
        var randomNumber = new byte[32]; using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomNumber); return Convert.ToBase64String(randomNumber);
        }
    }

    private ClaimsPrincipal GetPrincipalFromExpiredToken(string AccessToken)
    {
        //GetPrincipalFromExpiredToken is used to get the user principal from the expired access token.
        //We make use of the ValidateToken method from the JwtSecurityTokenHandler class for this purpose

        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = true,
            ValidateIssuer = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("SECRET")!)),
            ValidateLifetime = false,
            ValidIssuer = _jwtConfiguration.ValidIssuer,
            ValidAudience = _jwtConfiguration.ValidAudience
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        SecurityToken securityToken;
        var principal = tokenHandler.ValidateToken(AccessToken, tokenValidationParameters, out securityToken);
        var jwtSecurityToken = securityToken as JwtSecurityToken;
        if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
        {
            throw new SecurityTokenException("Invalid token");
        }
        return principal;
    }
}
