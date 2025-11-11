using Microsoft.AspNetCore.Components;

namespace Web.Blazor.Components;

public partial class Entry
{
    [Parameter] public string AutoComplete { get; set; } = "off";
    [Parameter] public string Label { get; set; } = string.Empty;
    [Parameter] public string Type { get; set; } = "text"; 
    [Parameter] public EventCallback<string> ValueChanged { get; set; }
    [Parameter] public string Value { get => field; set => SetValue(ref field, value); }
    private string field = string.Empty;

    private void SetValue(ref string field, string value)
    {
        if (field == value) return;
        field = value;
        ValueChanged.InvokeAsync(value);
    }
}