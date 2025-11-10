using Microsoft.AspNetCore.Components;
using Shared.Enums;
using Shared.Responses;

namespace Web.Blazor.Components;

public partial class BaseForm<TRequest, TResponse>
    : ComponentBase
    where TRequest : class
    where TResponse : Response
{
    [Parameter] public string Title { get; set; } = string.Empty;
    [Parameter] public string SubmitTitle { get; set; } = "Submit";
    [Parameter] public string LoadingTitle { get; set; } = "Submitting...";
    [Parameter] public string NavTitle { get; set; } = string.Empty;
    [Parameter] public string? ButtonNavigateTo { get; set; }
    [Parameter] public string? SubmitNavigateTo { get; set; }
    [Parameter] public TRequest Request { get; set; } = null!;
    [Parameter] public TResponse Response { get; set; } = null!;
    [Parameter] public RenderFragment<TRequest>? Content { get; set; }
    [Inject] private NavigationManager Navigation { get; init; } = null!;
    private bool IsLoading => Response?.IsLoading ?? false;
    private TResponse? previousResponse;
    private bool isSubscribed;

    protected override void OnParametersSet()
    {
        if (!ReferenceEquals(Response, previousResponse))
        {
            previousResponse = Response;
            isSubscribed = false;
        }

        if (Response is not null && !isSubscribed)
        {
            Action? previous = Response.Update;
            Response.Update = () =>
            {
                previous?.Invoke();
                _ = InvokeAsync(StateHasChanged);
            };
            isSubscribed = true;
        }
    }

    private async Task OnSubmitClickedAsync()
    {
        try
        {
            await Response.DoRequestAsync();

            if (!string.IsNullOrEmpty(SubmitNavigateTo) && Response.IsSuccess)
            {
                Navigation.NavigateTo(SubmitNavigateTo);
            }
        }
        catch
        {
            Response.Status = EResponseStatus.UnknownError;
        }
    }
}