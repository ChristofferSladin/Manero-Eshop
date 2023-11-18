using DataAccessLibrary.Entities.UserEntities;
using Microsoft.AspNetCore.Mvc;
using ServiceLibrary.Models;
using ServiceLibrary.Services;
using System.Net;

namespace ManeroWebApp.Tests.Services;

public class ProductServiceTests
{
    private readonly ProductService _productService = new(new HttpClient());

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

    [Fact]
    public async Task GetOnSaleProductsWithReviewsAsync_Returns_Only_OnSale_Products()
    {
        // Act
        var result = await _productService.GetOnSaleProductsWithReviewsAsync();

        // Assert
        Assert.NotEmpty(result);
        Assert.All(result, product => Assert.True(product.IsOnSale));
    }

    [Fact]
    public async Task GetFeaturedProductsWithReviewsAsync_Returns_Only_Featured_Products()
    {
        // Act
        var result = await _productService.GetFeaturedProductsWithReviewsAsync();

        // Assert
        Assert.NotEmpty(result);
        Assert.All(result, product => Assert.True(product.IsFeatured));
    }

    [Fact]
    public async Task GetOnSaleProductsWithReviewsAsync_Returns_Products_If_API_Online()
    {
        // Act
        var result = await _productService.GetOnSaleProductsWithReviewsAsync();

        // Assert
        Assert.NotEmpty(result);
    }

    [Fact]
    public async Task GetFeaturedProductsWithReviewsAsync_Returns_Products_If_API_Online()
    {
        // Act
        var result = await _productService.GetFeaturedProductsWithReviewsAsync();

        // Assert
        Assert.NotEmpty(result);
    }


    //Feactured Product Tests
    [Fact]
    public async Task GetProductsWithReviewsAsync_Return_ValidData()
    {
        // Act
        var result = await _productService.GetProductsWithReviewsAsync();

        // Assert
        foreach (var product in result)
        {
            Assert.NotNull(product.Description);
        }
    }

    [Fact]
    public async Task GetProductsWithReviewsAsync_Return_ValidReviewData()
    {
        // Act
        var result = await _productService.GetProductsWithReviewsAsync();

        // Assert
        foreach (var product in result)
        {
            foreach (var review in product.Reviews)
            {
                Assert.NotNull(review.Id);
                Assert.InRange(review.Rating, 0, 5);
            }
        }
    }

    [Fact]
    public async Task GetProductsWithReviewsAsync_Return_ValidProductData()
    {
        // Act
        var result = await _productService.GetProductsWithReviewsAsync();

        // Assert
        foreach (var product in result)
        {
            Assert.NotNull(product.ProductId);
            Assert.NotNull(product.ProductNumber);
            Assert.InRange(product.SalePricePercentage, 0, decimal.MaxValue);
        }
    }


    [Fact]
    public async Task GetProductsWithReviewsAsync_Return_NonNullProductFields()
    {
        // Act
        var result = await _productService.GetProductsWithReviewsAsync();

        // Assert
        foreach (var product in result)
        {
            Assert.NotNull(product.ProductId);
            Assert.NotNull(product.ProductName);
            Assert.NotNull(product.Description);
            Assert.NotNull(product.Category);
            // Add other assertions for other fields as needed
        }
    }

    [Fact]
    public async Task GetProductsWithReviewsAsync_Return_ValidPriceRange()
    {
        // Act
        var result = await _productService.GetProductsWithReviewsAsync();

        // Assert
        foreach (var product in result)
        {
            Assert.InRange(product.PriceExcTax, 0, decimal.MaxValue);
            Assert.InRange(product.PriceIncTax, 0, decimal.MaxValue);
            Assert.InRange(product.SalePricePercentage, 0, decimal.MaxValue);
        }
    }
}