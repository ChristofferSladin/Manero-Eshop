﻿using DataAccessLibrary.Entities.ProductEntities;
using DataAccessLibrary.Entities.UserEntities;
using DataAccessLibrary.Repositories;

namespace UserAPI.DTO;

public class FavoriteProductDto
{
    public int ProductId { get; set; }
    public string? ImgUrl { get; set; }
    public string? Name { get; set; }
    public decimal PriceWithTax { get; set; }
    public decimal PriceWithoutTax { get; set; }
    public decimal? SalePricePercentage { get; set; }
    public decimal? Rating { get; set; }
    public bool? IsOnSale { get; set; }
    public string UserId { get; set; }
    public int ShoppingCartId { get; set; }

    public static implicit operator FavoriteProductDto(FavoriteProduct favProduct)
    {
        return new FavoriteProductDto
        {
            ProductId = favProduct.ProductId,
            ImgUrl = favProduct.Product.ImageUrl,
            Name = favProduct.Product.ProductName,
            SalePricePercentage = favProduct.Product.SalePricePercentage,
            PriceWithTax = favProduct.Product.PriceIncTax,
            PriceWithoutTax = favProduct.Product.PriceExcTax,
            Rating = favProduct.Product.Rating,
            IsOnSale = favProduct.Product.IsOnSale,      
        };
    }
}