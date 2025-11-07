using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Models;

public class OrderProduct
{
    #region Keys
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public int OrderId { get; set; }
    public int ProductId { get; set; }
    #endregion

    #region Columns
    public int Count { get; set; }
    #endregion


    #region Include
    [ForeignKey(nameof(OrderId))]
    public Order? Order { get; set; }

    [ForeignKey(nameof(ProductId))]
    public Product? Product { get; set; }
    #endregion
}