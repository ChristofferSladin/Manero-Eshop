using DataAccessLibrary.Entities.ProductEntities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http.Json;

namespace ServiceLibrary.ProductServices;

public class ProductService : IProductService
{
    public async Task<List<Product>?> GetProductsAsync()
    {
        var products = new List<Product>();
        var baseUrl = "https://localhost:7067/products";
        try
        {
            using var client = new HttpClient();
            var request = new HttpRequestMessage();
            request.RequestUri = new Uri(baseUrl);
            request.Method = HttpMethod.Get;
            var response = await client.SendAsync(request);
            var responseString = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                products = JsonConvert.DeserializeObject<List<Product>>(responseString);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.InnerException);
        }

        return products;
    }
}
