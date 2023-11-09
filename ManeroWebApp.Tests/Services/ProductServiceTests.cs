using DataAccessLibrary.Entities.UserEntities;
using Microsoft.AspNetCore.Mvc;
using ServiceLibrary.Models;
using ServiceLibrary.Services;

namespace ManeroWebApp.Tests.Services;

public class ProductServiceTests
{
    private readonly ProductService _productService = new();

    [Fact]
    public async Task Get_ProductAsync_By_ProductNumber_Returns_Valid_Product()
    {
        var productNumber = 1.ToString("D12");
        var result = await _productService.GetProductAsync(productNumber);
        Assert.NotNull(result.ProductNumber);
        Assert.False(string.IsNullOrEmpty(result.ProductNumber));
    }

    [Fact]
    public async Task Get_ProductAsync_By_Invalid_ProductNumber_Returns_InValid_Product()
    {
        var productNumber = "ABC123";
        var result = await _productService.GetProductAsync(productNumber);
        Assert.Null(result.ProductNumber);
        Assert.True(string.IsNullOrEmpty(result.ProductNumber));
    }

    [Fact]
    public async Task Get_ProductFilteredProductsAsync_OrderByProperty_Asc_Returns_Valid_List_Of_Products_Ordered_By_Property()
    {
        var orderBy = "productNumber";
        var orderDirection = "asc";
        var result = await _productService.GetFilteredProductsAsync(null, null, null, orderBy, orderDirection, null);
        var orderedList = result.OrderBy(product => product.ProductNumber).ToList();
        Assert.Equal(result, orderedList);
    }

    [Fact]
    public async Task Get_FilteredProductsAsync_OrderByProperty_Desc_Returns_Valid_List_Of_Products_Ordered_By_Property()
    {
        var orderBy = "productNumber";
        var orderDirection = "desc";
        var result = await _productService.GetFilteredProductsAsync(null, null, null, orderBy, orderDirection, null);
        var orderedList = result.OrderByDescending(product => product.ProductNumber).ToList();
        Assert.Equal(result, orderedList);
    }

    [Theory]
    [InlineData("x", "x")]
    [InlineData("productNumber", "x")]
    [InlineData("productNumber", "")]
    [InlineData("x", "asc")]
    [InlineData("", "asc")]
    [InlineData("", "")]
    public async Task Get_FilteredProductsAsync_InvalidOrderByProperty_InvalidOrderBy_Returns_Valid_List_Of_Unordered_Products(string orderBy, string orderDirection)
    {
        var result = await _productService.GetFilteredProductsAsync(null, null, null, orderBy, orderDirection, null);
        Assert.NotNull(result);
    }

    [Theory]
    [InlineData(1, 2)]
    [InlineData(2, 3)]
    [InlineData(3, 2)]
    public async Task Get_FilteredProductsAsync_Returns_PaginatedResult(int page, int take)
    {
        var result = await _productService.GetFilteredProductsAsync(page, take, null, null, null, null);
        var skip = (page - 1) * take;
    
            Assert.Equal(take, result.Count);
            Assert.True(result.First().ProductId >= skip + 1);
    }

    [Fact]
    public async Task Get_FilteredProductsAsync_Filtered_By_Name_Returns_Products_Filtered_By_Name()
    {
        var productName = "Denim Jacket";
        var result = await _productService.GetFilteredProductsAsync(null, null, null, null, null, productName);
        if (result.Any())
        {
            foreach (var product in result)
            {
                Assert.Equal(productName, product.ProductName);
            }
        }
        else
        {
            Assert.Empty(result);
        }
    }

}