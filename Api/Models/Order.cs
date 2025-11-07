using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Models;

public class Order
{
    #region Keys
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string? UserId { get; set; }
    #endregion

    #region Include
    [ForeignKey(nameof(UserId))]
    public User? User { get; set; }
    public ICollection<OrderProduct>? OrderProducts { get; set; }
    #endregion
}