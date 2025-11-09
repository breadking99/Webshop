using Microsoft.AspNetCore.Mvc;
using Shared.Models;

namespace Shared.Interfaces;

//! orders
public interface IOrderController
{
     //! my
     Task<ActionResult<List<Order>>> GetMyOrdersAsync();
     //!
     Task<IActionResult> PostOrderAsync(Order request);
}