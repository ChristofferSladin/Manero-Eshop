using DataAccessLibrary.Entities.ProductEntities;

namespace ServiceLibrary.ProductServices
{
    public interface IProductService
    {
        Task<List<Product>?> GetProductsAsync();

    }
}