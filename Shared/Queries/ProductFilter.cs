namespace Shared.Queries;

public class ProductFilter : PagerQuery
{
    public string? NameContains { get; set; }
    public bool OnlyAvailable { get; set; }
}