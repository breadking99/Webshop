using Microsoft.AspNetCore.Components;
using Shared.Responses;
using System.Diagnostics.Contracts;

namespace Web.Blazor.Components;

public partial class BaseForm<TRequest, TResponse>
    : ComponentBase
    where TRequest : class
    where TResponse : Response
{
    [Parameter] public string Title { get; set; } = string.Empty;
    [Parameter] public string NavTitle { get; set; } = string.Empty;
    [Parameter] public string? ButtonNavigateTo { get; set; }
    [Parameter] public string? SubmitNavigateTo { get; set; }
    [Parameter] public TRequest Request { get; set; } = null!;
    [Parameter] public TResponse Response { get; set; } = null!;
    [Parameter] public RenderFragment<TRequest>? Content { get; set; }
    [Inject] private NavigationManager Navigation { get; init; } = null!;
    private bool IsLoading => Response?.IsLoading ?? false;

    protected override void OnInitialized()
    {
        base.OnInitialized();
    }

    private async Task OnSubmitClickedAsync()
    {
        try
        {
            await Response.DoRequestAsync();

            if (!string.IsNullOrEmpty(SubmitNavigateTo))
            {
                Navigation.NavigateTo(SubmitNavigateTo);
            }
        }
        catch { }
    }
}