using Shared.Models;
using Shared.Responses;

namespace Shared.Interfaces;

//! orders
public interface IOrderService
{
     //! my
     Task<Response<List<Order>>> GetMyOrdersAsync();
     //!
     Task<Response> PostOrderAsync(Order request);
}