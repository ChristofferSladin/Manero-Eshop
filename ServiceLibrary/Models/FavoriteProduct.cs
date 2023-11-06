namespace ServiceLibrary.Models;

public class FavoriteProduct
{
    public int ProductId { get; set; }
    public string UserId { get; set; }
    public int ShoppingCartId { get; set; }
    public string? ImgUrl { get; set; }
    public string? Name { get; set; }
    public decimal PriceWithTax { get; set; }
    public decimal PriceWithoutTax { get; set; }
    public decimal? SalePrice { get; set; }
    public decimal? Rating { get; set; }
    public bool? IsOnSale { get; set; }
}
