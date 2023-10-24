using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLibrary.Entities.ProductEntities;

public class ProductColor
{
    [Key]
    public int ProductColorId { get; set; }

    [ForeignKey(nameof(ProductId))]
    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;

    [ForeignKey(nameof(ColorId))]
    public int ColorId { get; set; }
    public Color Color { get; set; } = null!;
}
