using Microsoft.AspNetCore.Components;
using Shared.Interfaces;
using Shared.Models;
using Shared.Queries;
using Shared.Responses;
using Web.Blazor.Managers;

namespace Web.Blazor.Pages;

public partial class ProductsPage
{
    [Inject] IProductService ProductService { get; set; } = null!;
    [Inject] NavigationManager Navigation { get; set; } = null!;
    private ProductFilter Filter { get; set; } = new() { Number = 1, Size = 12 };
    private readonly Response<List<Product>> Response = new();
    private IReadOnlyList<Product>? ProductList => Response.Value;
    private bool CanPrev => Filter.Number > 1;

	protected override void OnInitialized()
    {
        Response.Update = () => _ = InvokeAsync(StateHasChanged);

        if (!DataManager.IsLoggedIn)
        {
            Navigation.NavigateTo("/login", true);
            return;
        }

        Response.Request = () => ProductService.GetProductsAsync(Filter);
    }

    protected override async Task OnInitializedAsync()
    {
        if (!DataManager.IsLoggedIn) return;
        await Response.DoRequestAsync();
    }

    private async Task PrevPage()
    {
        if (!CanPrev) return;
        Filter.Number--;
        await Response.DoRequestAsync();
    }

    private async Task NextPage()
    {
        if (ProductList is null || ProductList.Count < Filter.Size) return; // likely last page
        Filter.Number++;
        await Response.DoRequestAsync();
    }
}