using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLibrary.Entities.ProductEntities;

public class Color
{
    [Key]
    public int ColorId { get; set; }
    [Required]
    [Column(TypeName = "nvarchar(50)")]
    public string ColorName { get; set; } = null!;
    public virtual ICollection<ProductColor>? ProductColors { get; set; }
}
