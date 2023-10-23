using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace DataAccessLibrary.Entities;

public class ProductColor
{
    [Key]
    public int ProductColorId { get; set; }

    [ForeignKey("ProductId")]
    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;

    [ForeignKey("ColorId")]
    public int ColorId { get; set; }
    public Color Color { get; set; } = null!;
}
