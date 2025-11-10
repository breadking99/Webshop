using Shared.Interfaces;
using Shared.Requests;
using Shared.Responses;
using System.Text;

namespace Web.Blazor.Services;

public class AuthService(HttpClient httpClient) : BaseService(httpClient), IAuthService
{
    protected override StringBuilder GetServiceAddress(params object[] parameters)
        => base.GetServiceAddress("auth");

    public async Task<Response<string>> PostLoginAsync(LoginRequest request)
        => await PostAsync<LoginRequest, string>(request, ["login"]);

    public async Task<Response<string>> PostRegisterAsync(RegisterRequest request)
        => await PostAsync<RegisterRequest, string>(request, ["register"]);
}