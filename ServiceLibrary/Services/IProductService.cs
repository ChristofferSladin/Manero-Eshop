﻿using ServiceLibrary.Models;

namespace ServiceLibrary.Services
{
    public interface IProductService
    {
        Task<List<Product>> GetProductsWithReviewsAsync();
        Task<Product> GetProductAsync(string productNumber);
        Task<List<Product>> GetFilteredProductsAsync(int? page, int? take, string? category, string? orderBy,
            string? orderDirection, string? filterByName);

        Task<List<Product>> GetFilteredProductsWithReviewsAsync(int? page, int? take, string? category, string? orderBy,
            string? orderDirection, string? filterByName);
        Task<DataAccessLibrary.Entities.ProductEntities.Product> GetProductByIdAsync(int id);
        Task<List<Product>> GetFilteredProductsWithGenderAsync(int? page, int? take, string? gender, string? orderBy, string? orderDirection, string? filterByName);
    }
}