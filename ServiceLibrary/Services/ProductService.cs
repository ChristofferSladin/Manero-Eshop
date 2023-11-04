using System.Diagnostics;
using System.Drawing;
using ServiceLibrary.Models;
using System.Net.Http;
using Newtonsoft.Json.Linq;

namespace ServiceLibrary.Services;

public class ProductService : IProductService
{
    public async Task<List<Product>> GetProductsWithReviewsAsync()
    {
        var products = new List<Product>();
        try
        {
            var baseUrl = "https://localhost:7067/products/reviews";
            using var client = new HttpClient();
            var request = new HttpRequestMessage();
            request.RequestUri = new Uri(baseUrl);
            request.Method = HttpMethod.Get;
            var response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                var responseString = await response.Content.ReadAsStringAsync();
                dynamic jsonArray = JArray.Parse(responseString);
                foreach (var product in jsonArray)
                {
                    products.Add(product.ToObject<Product>());
                }
            }
        }
        catch (Exception ex) { Debug.WriteLine(ex.Message); }
        return products;
    }
}

