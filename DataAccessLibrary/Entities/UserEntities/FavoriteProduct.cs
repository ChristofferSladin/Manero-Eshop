using DataAccessLibrary.Entities.ProductEntities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DataAccessLibrary.Entities.UserEntities;

public class FavoriteProduct
{
    [Key]
    public int FavoriteProductId { get; set; }



    [ForeignKey(nameof(ProductId))]
    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;

    [ForeignKey(nameof(FavoriteId))]
    public int FavoriteId { get; set; }
    public Favorite Favorite { get; set; } = null!;
}