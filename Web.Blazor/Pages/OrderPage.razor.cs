using Microsoft.AspNetCore.Components;
using Shared.Enums;
using Shared.Interfaces;
using Shared.Models;
using Shared.Responses;
using Web.Blazor.Managers;

namespace Web.Blazor.Pages;

public partial class OrderPage
{
    private readonly Response SubmitResponse = new();
    private bool subscribed;
    [Inject] IOrderService OrderService { get; set; } = null!;
    [Inject] NavigationManager Navigation { get; set; } = null!;

    private bool IsSubmitting => SubmitResponse.IsLoading;
    private static IReadOnlyList<OrderProduct> Items => DataManager.CurrentOrder.OrderProducts?.ToList() ?? [];

    protected override void OnInitialized()
    {
        if (!DataManager.IsLoggedIn)
        {
            Navigation.NavigateTo("/login", true);
            return;
        }

        SubmitResponse.Update = () => _ = InvokeAsync(StateHasChanged);
        SubmitResponse.Request = SubmitOrderAsync;
        DataManager.StateChanged += OnStateChanged;
        subscribed = true;
    }

    private void OnStateChanged()
        => _ = InvokeAsync(StateHasChanged);

    private async Task SubmitAsync()
    {
        if (SubmitResponse.Request is null)
        {
            return;
        }

        await SubmitResponse.DoRequestAsync();

        if (SubmitResponse.IsSuccess)
        {
            if (string.IsNullOrWhiteSpace(SubmitResponse.Message))
            {
                SubmitResponse.Message = "Order submitted successfully.";
            }
            DataManager.ResetOrder();
        }
    }

    private Task<Response> SubmitOrderAsync()
    {
        if (!DataManager.HasOrderItems)
        {
            return Task.FromResult(new Response(EResponseStatus.BadRequest, "Your basket is empty."));
        }

        Order request = DataManager.CreateOrderRequest();
        if (request.OrderProducts is null || request.OrderProducts.Count == 0)
        {
            return Task.FromResult(new Response(EResponseStatus.BadRequest, "Your basket is empty."));
        }

        return OrderService.PostOrderAsync(request);
    }

    private void OnCountChanged(int productId, ChangeEventArgs args)
    {
        if (args.Value is null) return;
        if (!int.TryParse(args.Value.ToString(), out int count)) return;
        DataManager.SetProductCount(productId, count);
    }

    private void RemoveProduct(int productId)
        => DataManager.RemoveProduct(productId);

    private void ClearOrder()
        => DataManager.ResetOrder();

    public void Dispose()
    {
        if (subscribed)
        {
            DataManager.StateChanged -= OnStateChanged;
        }
    }
}