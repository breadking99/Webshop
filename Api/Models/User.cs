using Microsoft.AspNetCore.Identity;

namespace Api.Models;

public class User : IdentityUser
{
    #region Include
    public ICollection<Order> Orders { get; set; } = [];
    #endregion
}