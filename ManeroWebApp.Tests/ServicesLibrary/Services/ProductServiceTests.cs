using DataAccessLibrary.Entities.ProductEntities;
using FluentAssertions;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using ServiceLibrary.Services;
using System.Net;
using System.Text;
using Xunit.Abstractions;

namespace ManeroWebApp.Tests.ServicesLibrary.Services;

public class ProductServiceTests
{
    private readonly ProductService _productService = new(new HttpClient());
    private readonly ITestOutputHelper _output;

    public ProductServiceTests(ITestOutputHelper output)
    {
        _output = output;
    }

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


    //Product Categories Tests
    [Fact]
    public async Task GetGenderCategoriesAsync_Returns_ListOfString_For_GenderCategories_For_Products()
    {
        //Arrange
        var _httpMessageHandler = new Mock<HttpMessageHandler>();
        var _httpClient = new HttpClient(_httpMessageHandler.Object);
        IProductService _productService = new ProductService(_httpClient);
        var _genderCategories = new List<string> { "Men", "Women", "Kids", "Girls" };
        var successfulResponse = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent(JsonConvert.SerializeObject(_genderCategories))
        };

        _httpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(x =>
                    x.RequestUri == new Uri("https://localhost:7067/products/genderCategories")),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(successfulResponse);


        //Act
        var genderCategories = await _productService.GetGenderCategoriesAsync();

        //Assert
        Assert.NotEmpty(genderCategories);
        Assert.IsType<List<string>>(genderCategories);
        Assert.Equal(_genderCategories, genderCategories);
    }
    [Fact]
    public async Task GetProductSubCategoriesAsync_Returns_ListOfCategoryModel_For_ProductCategories_For_Men_Gender()
    {
        //Arrange
        var genderCategory = "Men";
        var _httpMessageHandler = new Mock<HttpMessageHandler>();
        var _httpClient = new HttpClient(_httpMessageHandler.Object);
        IProductService _productService = new ProductService(_httpClient);
        var _categories = new List<DataAccessLibrary.Entities.ProductEntities.Category>();
        switch (genderCategory)
        {
            case "Men":
                _categories.AddRange(new List<DataAccessLibrary.Entities.ProductEntities.Category>
                {
                    new DataAccessLibrary.Entities.ProductEntities.Category {CategoryName = "Pants" },
                    new DataAccessLibrary.Entities.ProductEntities.Category {CategoryName = "T-Shirts" },
                    new DataAccessLibrary.Entities.ProductEntities.Category {CategoryName = "Shoes" }
                });
                break;
            case "Women":
                _categories.AddRange(new List<DataAccessLibrary.Entities.ProductEntities.Category>
                {
                    new DataAccessLibrary.Entities.ProductEntities.Category {CategoryName = "Sweaters" },
                    new DataAccessLibrary.Entities.ProductEntities.Category {CategoryName = "Accessories" },
                    new DataAccessLibrary.Entities.ProductEntities.Category {CategoryName = "Jackets" }
                });
                break;
        }

        var successfulResponse = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent(JsonConvert.SerializeObject(_categories))
        };

        _httpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(x =>
                    x.RequestUri == new Uri($"https://localhost:7067/products/subcategories?genderCategory={genderCategory}")),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(successfulResponse);


        //Act
        var genderCategories = await _productService.GetProductSubCategoriesAsync(genderCategory);

        //Assert
        Assert.IsType<List<ServiceLibrary.Models.Category>>(genderCategories);
        Assert.NotEmpty(genderCategories);
    }

    [Fact]
    public async Task GetProductByIdAsync_SuccessfulResponse_ReturnsProduct()
    {
        // Arrange
        int productId = 1;
        var expectedProduct = new Product
        {
            ProductId = productId
        };

        var handlerMock = new Mock<HttpMessageHandler>();
        handlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req => req.RequestUri.ToString().Contains($"https://localhost:7067/product?id={productId}")),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonConvert.SerializeObject(expectedProduct)),
            });

        var httpClient = new HttpClient(handlerMock.Object);
        var productService = new ProductService(httpClient);

        // Act
        var result = await productService.GetProductByIdAsync(productId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedProduct.ProductId, result.ProductId);
    }

    [Fact]
    public async Task GetFilteredProductsWithReviewsAsync_SuccessfulResponse_ReturnsProductsWithReviews()
    {
        // Arrange
        int? page = null;
        int? take = null;
        string category = "Clothing";
        string orderBy = "ProductName";
        string orderDirection = "Asc";
        string filterByName = "";

        var expectedProducts = new List<Product>
    {
        new Product
        {
            ProductName = "ABC",
            Category = category,
            Reviews = new List<Review>
            {
                new Review { Id = "1", Rating = 4 },
                new Review { Id = "2", Rating = 5 }
            }
        },
        new Product
        {
            ProductName = "BCD",
            Category = category,
            Reviews = new List<Review>
            {
                new Review { Id = "3", Rating = 3 },
                new Review { Id = "4", Rating = 4 }
            }
        }
    };

        var handlerMock = new Mock<HttpMessageHandler>();
        handlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonConvert.SerializeObject(expectedProducts), Encoding.UTF8, "application/json"),
            });

        var httpClient = new HttpClient(handlerMock.Object);
        var productService = new ProductService(httpClient);

        // Act
        var result = await productService.GetFilteredProductsWithReviewsAsync(page, take, category, orderBy, orderDirection, filterByName);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(expectedProducts, options => options
            .ExcludingMissingMembers()); // Ignore missing members in the expectation
        foreach (var product in result)
        {
            Assert.NotNull(product.Reviews);
            Assert.NotEmpty(product.Reviews);
        }
    }
}
