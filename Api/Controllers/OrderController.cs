using Api.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared.Interfaces;
using Shared.Models;
using System.Security.Claims;

namespace Api.Controllers;

[Route("orders")]
[Authorize]
[ApiController]
public class OrderController(Context context) : ControllerBase, IOrderController
{
    #region Fields
    private readonly Context context = context;
    #endregion

    #region Methods
    [Authorize(Roles = "admin")]
    [HttpGet]
    public async Task<ActionResult<List<Order>>> GetProductsAsync()
    {
        List<Order> orders = await context.Orders
            .IncludeProducts()
            .ToListAsync();

        return Ok(orders);
    }

    [HttpGet("my")]
    public async Task<ActionResult<List<Order>>> GetMyOrdersAsync()
    {
        string userId = GetUserId();
        List<Order> orders = await context.Orders
            .Where(x => x.UserId == userId)
            .IncludeProducts()
            .ToListAsync();

        return Ok(orders);
    }

    [HttpPost]
    public async Task<IActionResult> PostOrderAsync(
        [FromBody] Order request)
    {
        request.UserId = GetUserId();
        //? Lets says the web prevents to add more from on Prodcut, than how many is in the store
        //? In that case, an issue only can occur, when more one person does an order in the same time,
        //? and from at least one product they are combined order more than the store capacity
        //? Lets resolve this later!
        try
        {
            context.Orders.Add(request);
            await context.SaveChangesAsync();
        }
        catch { return BadRequest(); }
        return Ok();
    }

    //!? Copied from my older project:
    private string GetUserId()
    {
        static bool predicate(Claim c) => c.Type == ClaimTypes.NameIdentifier;
        IEnumerable<Claim> claims = User.Claims;
        Claim? claim = claims.FirstOrDefault(predicate);
        string? loggedUserId = claim?.Value;

        return loggedUserId ?? string.Empty;
    }
    #endregion
}