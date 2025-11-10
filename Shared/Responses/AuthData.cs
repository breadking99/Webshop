using Shared.Interfaces;
using Shared.Requests;

namespace Shared.Responses;

public class AuthData : IMessage
{
    public AuthData() { }
    public AuthData(string username, string token, DateTimeOffset validTo)
    {
        Success = true;
        Username = username;
        Token = token;
        ValidTo = validTo;
    }

    public static AuthData FromResult(Response<AuthData> response, LoginRequest request) => new()
    {
        Success = response?.Value?.Success ?? false,
        Username = response?.Value?.Username,
        Email = request.Email,
        Password = request.Password,
        Token = response?.Value?.Token,
        ValidTo = response?.Value?.ValidTo ?? DateTimeOffset.MinValue
    };

    public bool Success { get; set; }
    public string? Message { get; set; }
    public string? Username { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }
    public string? Token { get; set; }
    public DateTimeOffset ValidTo { get; set; }
}