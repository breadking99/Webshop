using Shared.Interfaces;
using Shared.Models;
using Shared.Queries;
using Shared.Responses;
using System.Text;

namespace Web.Blazor.Services;

public class ProductService(HttpClient httpClient) : BaseService(httpClient), IProductService
{
    protected override StringBuilder GetServiceAddress(params object[] parameters)
        => base.GetServiceAddress("products");

    public Task<Response<List<Product>>> GetProductsAsync(ProductFilter? pager)
        => GetAsync<List<Product>, ProductFilter>(query: pager);

    public Task<Response<Product>> GetProductByIdAsync(int id)
        => GetAsync<Product>([id]);
}