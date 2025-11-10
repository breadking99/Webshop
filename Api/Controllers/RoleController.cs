using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shared.Models;

namespace Api.Controllers;

[Route("roles")]
[Authorize(Roles = "admin")]
[ApiController]
public class RoleController(RoleManager<IdentityRole> roleManager, UserManager<User> userManager)
    : ControllerBase
{
    #region Fields
    private readonly RoleManager<IdentityRole> roleManager = roleManager;
    private readonly UserManager<User> userManager = userManager;
    #endregion

    #region Methods (POST)
    [HttpPost("{roleName}")]
    public async Task<IActionResult> PostAsync(
        [FromRoute] string roleName)
    {
        IdentityRole? role = await roleManager.FindByNameAsync(roleName);

        if (role != null) return BadRequest("Role already exists!");

        role = new IdentityRole(roleName);
        await roleManager.CreateAsync(role);

        return Ok($"Role {roleName} created!");
    }

    [HttpPost("{roleName}/{userId}")]
    public async Task<IActionResult> PostRoleToUserAsync(
        [FromRoute] string roleName,
        [FromRoute] string userId)
    {
        User? user = await userManager.FindByIdAsync(userId);

        if (user == null) return BadRequest("User does not exist!");

        IdentityResult result = await userManager.AddToRoleAsync(user, roleName);

        if (result.Succeeded) return Ok($"Role {roleName} added to user {user.UserName}!");
        else return BadRequest("Failed to add role to user!");
    }
    #endregion
}