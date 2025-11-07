using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Models;

public class Product
{
    #region Keys
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    #endregion

    #region Columns
    [Column(TypeName = "nvarchar(50)")]
    public string Name { get; set; } = "New Product";
    [Column]
    public int Store { get; set; } = 100;
    #endregion

    #region Include
    public ICollection<OrderProduct>? OrderProducts { get; set; }
    #endregion

    #region Not Mapped
    //public bool IsAvailable => Store - OrderProduct.Sum(x => x.Count) > 0;
    #endregion
}
