﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ServiceLibrary.Models;
using System.Diagnostics;
using System.Text;
using System.Text.Json.Nodes;
using System.Reflection;

namespace ServiceLibrary.Services;

public class ProductService : IProductService
{
    private readonly HttpClient _httpClient;

    public ProductService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<Product>> GetProductsWithReviewsAsync()
    {
        var products = new List<Product>();
        try
        {
            var request = new HttpRequestMessage
            {
                RequestUri =  new Uri("https://localhost:7067/products/reviews"),
                Method = HttpMethod.Get
            };
            var response = await _httpClient.SendAsync(request);
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

    public async Task<List<Product>> GetOnSaleProductsWithReviewsAsync()
    {
        var products = new List<Product>();
        try
        {
            var request = new HttpRequestMessage
            {
                RequestUri =  new Uri("https://localhost:7067/products/onsale/reviews"),
                Method = HttpMethod.Get
            };
            var response = await _httpClient.SendAsync(request);
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

    public async Task<List<Product>> GetFeaturedProductsWithReviewsAsync()
    {
        var products = new List<Product>();
        try
        {
            var request = new HttpRequestMessage
            {
                RequestUri =  new Uri("https://localhost:7067/products/featured/reviews"),
                Method = HttpMethod.Get
            };
            var response = await _httpClient.SendAsync(request);
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
        try
        {
            var request = new HttpRequestMessage
            {
                RequestUri =  new Uri($"https://localhost:7067/product?productNumber={productNumber}"),
                Method = HttpMethod.Get
            };
            var response = await _httpClient.SendAsync(request);
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

            var request = new HttpRequestMessage
            {
                RequestUri = new Uri(baseUrl),
                Method = HttpMethod.Get,
            };
            var response = await _httpClient.SendAsync(request);
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

            var request = new HttpRequestMessage
            {
                RequestUri = new Uri(baseUrl),
                Method = HttpMethod.Get,
            };
            var response = await _httpClient.SendAsync(request);
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
            var response = await _httpClient.GetAsync($"https://localhost:7067/product?id={productId}");
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
    public async Task<List<Product>> GetFilteredProductsWithGenderAsync(int? page, int? take, string? gender, string? orderBy, string? orderDirection, string? filterByName)
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
            if (!string.IsNullOrEmpty(gender))
                query.Append($"filterByCategory={Uri.EscapeDataString(gender)}&");
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
    public async Task<List<Product>> GetFilteredProductsAsync(int? page, int? take, string? category, string? orderBy, string? orderDirection, string? filterByName, string? gender)
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
            if (!string.IsNullOrEmpty(gender))
                query.Append($"gender={Uri.EscapeDataString(gender)}&");
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
    public async Task<List<string>> GetGenderCategoriesAsync()
    {
        try
        {
            var baseUrl = new StringBuilder("https://localhost:7067/products/genderCategories");
            var request = new HttpRequestMessage();
            request.RequestUri = new Uri(baseUrl.ToString());
            request.Method = HttpMethod.Get;
            var response = await _httpClient.SendAsync(request);
            
            if (response.IsSuccessStatusCode)
            {
                var responseString = await response.Content.ReadAsStringAsync();
                var list = JsonConvert.DeserializeObject<List<string>>(responseString);
                if(list is not null)
                    return list;
            }
        }
        catch (Exception e){ Debug.WriteLine(e.Message); }
        
        return null!;
    }
    public async Task<List<Category>> GetProductSubCategoriesAsync(string genderCategory)
    {
        try
        {
            var baseUrl = new StringBuilder("https://localhost:7067/products/subcategories?");
            baseUrl.Append($"genderCategory={Uri.EscapeDataString(genderCategory)}");
            var request = new HttpRequestMessage();
            request.RequestUri = new Uri(baseUrl.ToString());
            request.Method = HttpMethod.Get;
            var response = await _httpClient.SendAsync(request);
            
            if (response.IsSuccessStatusCode)
            {
                var responseString = await response.Content.ReadAsStringAsync();
                var subcategoryList = JsonConvert.DeserializeObject<List<Category>>(responseString);
                if(subcategoryList is not null)
                    return subcategoryList;
            }
        }
        catch (Exception e){ Debug.WriteLine(e.Message); }
        
        return null!;
    }
}

