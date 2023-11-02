﻿using ServiceLibrary.Models;

namespace ServiceLibrary.Services
{
    public interface IProductService
    {
        Task<List<Product>> GetProductsWithReviewsAsync();
        Task<Product> GetProductWithReviewsAsync(int productId);

        Task<List<Product>> GetFilteredProductsAsync(int? page, int? take, string? category, string? orderBy,
            string? orderDirection, string? filterByName);
    }
}