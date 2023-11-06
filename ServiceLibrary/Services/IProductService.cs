using ServiceLibrary.Models;

namespace ServiceLibrary.Services
{
    public interface IProductService
    {
        Task<List<Product>> GetProductsWithReviewsAsync();
        Task<DataAccessLibrary.Entities.ProductEntities.Product> GetProductByIdAsync(int id);
    }
}