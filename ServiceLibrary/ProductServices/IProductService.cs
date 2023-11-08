
using ServiceLibrary.Models;

namespace ServiceLibrary.ProductServices
{
    public interface IProductService
    {
        Task<List<Product>?> GetProductsAsync();

    }
}