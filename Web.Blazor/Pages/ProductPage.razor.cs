using Microsoft.AspNetCore.Components;
using Shared.Interfaces;
using Shared.Models;
using Shared.Responses;
using Web.Blazor.Managers;

namespace Web.Blazor.Pages;

public partial class ProductPage
{
    [Inject] IProductService ProductService { get; set; } = null!;
    [Inject] NavigationManager Navigation { get; set; } = null!;
    [Parameter] public int Id { get; set; }

    private readonly Response<Product> ResponseModel = new();
    private int currentProductId = -1;
    private bool shouldLoad;
    private int quantity = 1;
    private string InfoMessage { get; set; } = string.Empty;
    private Product? CurrentProduct => ResponseModel.Value;
    private int MaxSelectable => CurrentProduct is null ? 1 : Math.Max(1, CurrentProduct.Store);
    private bool CanAdd => CurrentProduct is not null && CurrentProduct.Store > 0;

    protected override void OnParametersSet()
    {
        if (!DataManager.IsLoggedIn)
        {
            Navigation.NavigateTo("/login", true);
            return;
        }

        if (ResponseModel.Update is null)
        {
            ResponseModel.Update = () => _ = InvokeAsync(StateHasChanged);
        }


        if (currentProductId == Id)
        {
            return;
        }

        currentProductId = Id;
        ResponseModel.Request = () => ProductService.GetProductByIdAsync(Id);
        shouldLoad = true;
    }

    protected override async Task OnParametersSetAsync()
    {
        if (!DataManager.IsLoggedIn || !shouldLoad)
        {
            return;
        }

        shouldLoad = false;
        await ResponseModel.DoRequestAsync();
        quantity = 1;
        InfoMessage = string.Empty;
    }

    private void AddToBasket()
    {
        if (CurrentProduct is null)
        {
            return;
        }

        int available = Math.Max(0, CurrentProduct.Store);
        if (available == 0)
        {
            InfoMessage = "This product is currently out of stock.";
            return;
        }

        int selected = Math.Clamp(quantity, 1, available);
        DataManager.IncrementProduct(CurrentProduct, selected);
        quantity = selected;
        InfoMessage = selected == 1
            ? "Added 1 item to your basket."
            : $"Added {selected} items to your basket.";
    }
}