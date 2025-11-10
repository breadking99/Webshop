using Shared.Interfaces;
using Shared.Models;
using Shared.Responses;
using System.Text;

namespace Web.Blazor.Services;

public class OrderService(HttpClient httpClient) : BaseService(httpClient), IOrderService
{
    protected override StringBuilder GetServiceAddress(params object[] parameters)
        => base.GetServiceAddress("orders");

    public Task<Response<List<Order>>> GetMyOrdersAsync()
        => GetAsync<List<Order>>(["my"]);

    public Task<Response> PostOrderAsync(Order request)
        => PostAsync(request);
}
