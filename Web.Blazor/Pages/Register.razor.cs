using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Shared.Interfaces;
using Shared.Requests;
using Shared.Responses;
using Web.Blazor.Managers;
using Web.Blazor.Services;

namespace Web.Blazor.Pages;

public partial class Register
{
    [Inject] IAuthService AuthService { get; set; } = null!;
    [Inject] NavigationManager Navigation { get; set; } = null!;
    private readonly AuthRequest Request = new();
    private readonly Response<AuthData> Response = new();

    protected override void OnInitialized()
    {
        Response.Update = () => _ = InvokeAsync(StateHasChanged);

        if (DataManager.IsLoggedIn)
        {
            Navigation.NavigateTo("/", true);
            return;
        }

        Response.Request = () => AuthService.PostRegisterAsync(Request);
    }
}