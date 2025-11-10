using Shared.Interfaces;
using Shared.Requests;
using Shared.Responses;
using System.Text;
using Web.Blazor.Managers;

namespace Web.Blazor.Services;

public class AuthService(HttpClient httpClient) : BaseService(httpClient), IAuthService
{
    protected override StringBuilder GetServiceAddress(params object[] parameters)
        => base.GetServiceAddress("auth");

    public async Task<Response<AuthData>> PostLoginAsync(LoginRequest request)
    {
        var response = await PostAsync<LoginRequest, AuthData>(request, ["login"]);
        DataManager.AuthData = AuthData.FromResult(response, request);

        return response;
    }

    public async Task<Response<AuthData>> PostRegisterAsync(RegisterRequest request)
    {
        var response = await PostAsync<RegisterRequest, AuthData>(request, ["register"]);
        DataManager.AuthData = AuthData.FromResult(response, request);

        return response;
    }
}