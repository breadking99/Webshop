using Api.Contracts;
using Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers;

[Route("products")]
[AllowAnonymous]
[ApiController]
public class ProductController(Context context) : ControllerBase
{
    #region Fields
    private readonly Context context = context;
    #endregion
    #region Methods
    [HttpGet]
    public async Task<ActionResult<List<Product>>> GetProductsAsync(
        [FromQuery] Pager? pager)
    {
        List<Product> products = await context.Products
            .Skip(pager is null ? 0 : (pager.Number - 1) * pager.Size)
            .Take(pager is null ? 1000 : pager.Size)
            .ToListAsync();

        return Ok(products);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Product>> GetProductByIdAsync(
        [FromRoute] int id)
    {
        Product? product = await context.Products
            .Where(x => x.Id == id)
            .FirstOrDefaultAsync();

        if (product is null) return NotFound();
        return Ok(product);
    }

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