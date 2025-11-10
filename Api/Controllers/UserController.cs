using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shared.Models;

namespace Api.Controllers;

[Route("users")]
[Authorize(Roles = "admin")]
[ApiController]
public class UserController(UserManager<User> userManager) : ControllerBase
{
    #region Fields
    private readonly UserManager<User> userManager = userManager;
    #endregion

    #region Methods (GET)
    [HttpGet]
    public async Task<ActionResult<IEnumerable<User>>> GetUsersAsync()
    {
        List<User> users = [.. userManager.Users
            .Select(x => new User
            {
                Id = x.Id,
                Email = x.Email,
                UserName = x.UserName,
                SecurityStamp = null,
                ConcurrencyStamp = null,
            })];

        foreach (User user in users)
        {
            user.Roles = [.. await userManager.GetRolesAsync(
                await userManager.FindByIdAsync(user.Id) ?? new())];
        }

        return Ok(users);
    }
    #endregion
}
