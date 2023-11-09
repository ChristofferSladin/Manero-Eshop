using DataAccessLibrary.Entities.UserEntities;

namespace UserAPI.DTO;

public class FavoriteDto
{
    public int FavoriteProductId { get; set; }
    public int ProductId { get; set; }
    public int FavoriteId { get; set; }
    public static implicit operator FavoriteDto(FavoriteProduct favoriteProduct)
    {
        return new FavoriteDto
        {
            FavoriteProductId = favoriteProduct.FavoriteProductId,
            FavoriteId = favoriteProduct.FavoriteId,
            ProductId = favoriteProduct.ProductId
        };
    }
}
