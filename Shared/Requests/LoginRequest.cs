namespace Shared.Requests;

public class LoginRequest
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public class RegisterRequest : LoginRequest
{
    public string ConfirmPassword { get; set; } = string.Empty;
}