using Microsoft.AspNetCore.Identity;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Models;

public class User : IdentityUser
{
    #region Include
    public ICollection<Order>? Orders { get; set; }
    #endregion

    #region Not Mapped
    [NotMapped]
    public ICollection<string>? Roles { get; set; }
    #endregion
}