using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Shared.Models;
using Shared.Requests;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Api.Controllers;

[Route("auth")]
[AllowAnonymous]
[ApiController]
public class AuthController(UserManager<User> userManager, IConfiguration configuration) : ControllerBase
{
    #region Fields
    private readonly UserManager<User> userManager = userManager;
    private readonly IConfiguration configuration = configuration;
    #endregion

    #region Methods
    [HttpPost("login")]
    public async Task<ActionResult<string>> PostLoginAsync(
        [FromBody] LoginRequest request)
    {
        string? token = await LoginAsync(request);

        if (token == null) return Unauthorized("Unsuccessful login attempt.");
        return Ok(token);
    }

    [HttpPost("register")]
    public async Task<ActionResult<string>> PostRegisterAsync(
        [FromBody] RegisterRequest request)
    {
        if (request.Password != request.ConfirmPassword)
            return BadRequest("Password and Confirm Password do not match.");

        string? token = await RegisterAsync(request);

        if (token == null) return Unauthorized("Unsuccessful registration attempt.");
        return Ok(token);
    }

    private async Task<string?> RegisterAsync(RegisterRequest request)
    {
        // Is email exist:
        User? user = await userManager.FindByEmailAsync(request.Email);

        if (user != null) return null;

        // CreateClaims new user:
        user = new()
        {
            Email = request.Email,
            UserName = request.Email,
            SecurityStamp = Guid.NewGuid().ToString()
        };

        IdentityResult result = await userManager.CreateAsync(user, request.Password);

        // Return result:
        if (result == IdentityResult.Success) return await LoginAsync(request);
        else return null;
    }

    private async Task<string?> LoginAsync(LoginRequest request)
    {
        // Validate user credentials:
        User? user = await ValidateUserCredentialsAsync(request);

        if (user == null) return null;

        // Get data for creating token:
        List<Claim> claims = await CreateClaims(user);
        DateTimeOffset validTo = DateTimeOffset.Now.AddHours(1);
        DateTime expires = validTo.UtcDateTime;
        SigningCredentials signingCredentials = GetSigningCredentials();

        // Creating token:
        string token = CreateToken(claims, expires, signingCredentials);

        return token;
    }

    private async Task<User?> ValidateUserCredentialsAsync(LoginRequest request)
    {
        // Check if user exists:
        User? user = await userManager.FindByEmailAsync(request.Email);

        if (user == null)
        {
            return null;
        }

        // Check if user's email is confirmed:
        bool isEmailConfirmed = true; //await userManager.IsEmailConfirmedAsync(user);

        if (!isEmailConfirmed)
        {
            return null;
        }

        // Check if password is correct:
        bool isPasswordCorrect = await userManager.CheckPasswordAsync(user, request.Password);

        if (!isPasswordCorrect)
        {
            return null;
        }

        return user;
    }

    private async Task<List<Claim>> CreateClaims(User user)
    {
        List<Claim> claims = [new (ClaimTypes.NameIdentifier, user.Id) ];
        IList<string> roles = await userManager.GetRolesAsync(user);

        foreach (string role in roles)
        {
            claims.Add(new (ClaimTypes.Role, role));
        }

        return claims;
    }

    private SigningCredentials GetSigningCredentials()
    {
        string authenticationKey = configuration["Authentication:Key"]!;
        byte[] keyBytes = Encoding.UTF8.GetBytes(authenticationKey);
        SymmetricSecurityKey securityKey = new(keyBytes);
        SigningCredentials signingCredentials = new(securityKey, SecurityAlgorithms.HmacSha256);

        return signingCredentials;
    }

    private string CreateToken(List<Claim> claims, DateTime expires, SigningCredentials signingCredentials)
    {
        JwtSecurityToken token = new(
            issuer: configuration["Authentication:Issuer"],
            audience: configuration["Authentication:Audience"],
            claims: claims,
            expires: expires,
            signingCredentials: signingCredentials);
        JwtSecurityTokenHandler jwtSecurityTokenHandler = new();
        string tokenAsString = jwtSecurityTokenHandler.WriteToken(token);

        return tokenAsString;
    }
    #endregion
}