using ServiceLibrary.Models;

namespace ServiceLibrary.Services
{
    public interface IProductService
    {
        Task<List<Product>> GetProductsWithReviewsAsync();
        Task<List<Product>> GetOnSaleProductsWithReviewsAsync();
        Task<List<Product>> GetFeaturedProductsWithReviewsAsync();
        Task<Product> GetProductAsync(string productNumber);
        Task<List<Product>> GetFilteredProductsAsync(int? page, int? take, string? category, string? orderBy,
            string? orderDirection, string? filterByName);

        Task<List<Product>> GetFilteredProductsWithReviewsAsync(int? page, int? take, string? category, string? orderBy,
            string? orderDirection, string? filterByName);
        Task<DataAccessLibrary.Entities.ProductEntities.Product> GetProductByIdAsync(int id);
    }
}