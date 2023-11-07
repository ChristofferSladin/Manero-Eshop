using System.Diagnostics;
using System.Drawing;
using ServiceLibrary.Models;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Text;
using Newtonsoft.Json;
using System.Text.Json.Nodes;

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
    public async Task<Product> GetProductAsync(string productNumber)
    {
        var product = new Product();
        var pId = $"?productNumber={productNumber}";
        try
        {
            var baseUrl = $"https://localhost:7067/product{pId}";
            using var client = new HttpClient();
            var request = new HttpRequestMessage();
            request.RequestUri = new Uri(baseUrl);
            request.Method = HttpMethod.Get;
            var response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                var responseString = await response.Content.ReadAsStringAsync();
                product = JsonConvert.DeserializeObject<Product>(responseString);
            }
        }
        catch (Exception ex) { Debug.WriteLine(ex.Message); }
        return product;
    }
    public async Task<List<Product>> GetFilteredProductsAsync(int? page, int? take, string? category, string? orderBy, string? orderDirection, string? filterByName)
    {
        var products = new List<Product>();
        try
        {
            var uriBuilder = new UriBuilder("https://localhost:7067/products/filter");
            var query = new StringBuilder();
            if (page.HasValue)
                query.Append($"page={page.Value}&");
            if (take.HasValue)
                query.Append($"take={take.Value}&");
            if (!string.IsNullOrEmpty(category))
                query.Append($"filterByCategory={Uri.EscapeDataString(category)}&");
            if (!string.IsNullOrEmpty(orderBy))
                query.Append($"orderByField={Uri.EscapeDataString(orderBy)}&");
            if (!string.IsNullOrEmpty(orderDirection))
                query.Append($"orderDirection={Uri.EscapeDataString(orderDirection)}&");
            if (!string.IsNullOrEmpty(filterByName))
                query.Append($"filterByName={Uri.EscapeDataString(filterByName)}&");
            if (query.Length > 0)
                query.Length--;

            uriBuilder.Query = query.ToString();

            var baseUrl = uriBuilder.Uri.ToString();
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
    public async Task<List<Product>> GetFilteredProductsWithReviewsAsync(int? page, int? take, string? category, string? orderBy, string? orderDirection, string? filterByName)
    {
        var products = new List<Product>();
        try
        {
            var uriBuilder = new UriBuilder("https://localhost:7067/products/reviews/filter");
            var query = new StringBuilder();
            if (page.HasValue)
                query.Append($"page={page.Value}&");
            if (take.HasValue)
                query.Append($"take={take.Value}&");
            if (!string.IsNullOrEmpty(category))
                query.Append($"filterByCategory={Uri.EscapeDataString(category)}&");
            if (!string.IsNullOrEmpty(orderBy))
                query.Append($"orderByField={Uri.EscapeDataString(orderBy)}&");
            if (!string.IsNullOrEmpty(orderDirection))
                query.Append($"orderDirection={Uri.EscapeDataString(orderDirection)}&");
            if (!string.IsNullOrEmpty(filterByName))
                query.Append($"filterByName={Uri.EscapeDataString(filterByName)}&");
            if (query.Length > 0)
                query.Length--;

            uriBuilder.Query = query.ToString();

            var baseUrl = uriBuilder.Uri.ToString();
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
    public async Task<Product> GetProductByIdAsync(int productId)
    {
        try
        {
            using var httpClient = new HttpClient();
            var response = await httpClient.GetAsync($"https://localhost:7067/product?id={productId}");
            if (response.IsSuccessStatusCode)
            {
                var content = response.Content.ReadAsStringAsync().Result.ToString();
                var product = JsonConvert.DeserializeObject<Product>(content);

                if (product is not null)
                    return product;
            }
        }
        catch (Exception ex) { Debug.WriteLine(ex.Message); }
        return null!;
    }
}

