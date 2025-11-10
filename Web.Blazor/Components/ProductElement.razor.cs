using Microsoft.AspNetCore.Components;
using Shared.Models;

namespace Web.Blazor.Components;

public partial class ProductElement
{
    [Parameter] public Product Product { get; set; } = null!;
    private string NavigateTo => $"/products/{Product.Id}";
}