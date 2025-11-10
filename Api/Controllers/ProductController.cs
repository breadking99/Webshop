using Api.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared.Models;
using Shared.Queries;

namespace Api.Controllers;

[Route("products")]
[Authorize]
[ApiController]
public class ProductController(Context context) : ControllerBase
{
    #region Fields
    private readonly Context context = context;
    #endregion

    #region Methods
    [HttpGet]
    public async Task<ActionResult<List<Product>>> GetProductsAsync(
        [FromQuery] PagerQuery? pager)
    {
        IQueryable<Product> queryable = context.Products
            .IncludeOrderCounts();

        if (pager != null && pager.Number > 0 && pager.Size > 0)
        {
            queryable = queryable
                .Skip((pager.Number - 1) * pager.Size)
                .Take(pager.Size);
        }

        List<Product> products = await queryable.ToListAsync();

        return Ok(products);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Product>> GetProductByIdAsync(
        [FromRoute] int id)
    {
        Product? product = await context.Products
            .IncludeOrderCounts()
            .Where(x => x.Id == id)
            .FirstOrDefaultAsync();

        if (product is null) return NotFound();
        return Ok(product);
    }

    [Authorize(Roles = "admin")]
    [HttpPost]
    public async Task<IActionResult> PostProductAsync(
        [FromBody] Product product)
    {
        try
        {
            context.Products.Add(product);
            await context.SaveChangesAsync();

        }
        catch { return BadRequest(); }

        return Ok();
    }

    [Authorize(Roles = "admin")]
    [HttpPut("{id}")]
    public async Task<IActionResult> PutProductByIdAsync(
        [FromRoute] int id,
        [FromBody] Product product)
    {
        try
        {
            Product? old = await context.Products
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();

            if (old is null) return NotFound();

            old.Name = product.Name;
            old.Store = product.Store;

            await context.SaveChangesAsync();

        }
        catch { return BadRequest(); }

        return Ok();
    }
    #endregion
}