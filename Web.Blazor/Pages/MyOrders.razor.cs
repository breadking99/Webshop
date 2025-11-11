using Microsoft.AspNetCore.Components;
using Shared.Interfaces;
using Shared.Models;
using Shared.Responses;
using Web.Blazor.Managers;

namespace Web.Blazor.Pages;

public partial class MyOrders
{
    private readonly Response<List<Order>> Response = new();
    private IReadOnlyList<Order>? Orders => Response.Value;
    [Inject] IOrderService OrderService { get; set; } = null!;
    [Inject] NavigationManager Navigation { get; set; } = null!;

    protected override void OnInitialized()
    {
        Response.Update = () => _ = InvokeAsync(StateHasChanged);

        if (!DataManager.IsLoggedIn)
        {
            Navigation.NavigateTo("/login", true);
            return;
        }

        Response.Request = () => OrderService.GetMyOrdersAsync();
    }

    protected override async Task OnInitializedAsync()
    {
        if (!DataManager.IsLoggedIn) return;
        await Response.DoRequestAsync();
    }
}