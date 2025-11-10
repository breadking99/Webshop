using Microsoft.AspNetCore.Components;

namespace Web.Blazor.Components;

public partial class NavButton
{
    [Parameter] public string NavigateTo { get; set; } = string.Empty;
    [Parameter] public string Title { get; set; } = string.Empty;
    [Parameter] public string Class { get; set; } = "form-button create-account-button";
    [Inject] private NavigationManager Navigation { get; set; } = default!;

    private void OnClick() => Navigation.NavigateTo(NavigateTo);
}