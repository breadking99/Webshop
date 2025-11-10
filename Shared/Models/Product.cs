using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Models;

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
    public int Store { get; set; }
    #endregion

    #region Include
    public ICollection<OrderProduct>? OrderProducts { get; set; }
    #endregion

    #region Not Mapped
    [NotMapped]
    public int OrderedCount => OrderProducts?.Aggregate(0, (acc, x) => x.Count + acc) ?? 0;
    [NotMapped]
    public int AvailableCount => Store - OrderedCount;
    #endregion
}
