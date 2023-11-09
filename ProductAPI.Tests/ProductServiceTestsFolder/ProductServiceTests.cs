using ServiceLibrary.Services;
using ServiceLibrary.Models;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace ProductAPI.Tests.ProductServiceTestsFolder
{
    public class ProductServiceTests
    {
   

        [Fact]
        public async Task GetProductsWithReviewsAsync_ShouldReturnNonEmptyReviews()
        {
            // Arrange
            var productService = new ProductService();

            // Act
            var result = await productService.GetProductsWithReviewsAsync();

            // Assert
            foreach (var product in result)
            {
                Assert.NotEmpty(product.Reviews);
            }
        }

        [Fact]
        public async Task GetProductsWithReviewsAsync_ShouldReturnValidData()
        {
            // Arrange
            var productService = new ProductService();

            // Act
            var result = await productService.GetProductsWithReviewsAsync();

            // Assert
            foreach (var product in result)
            {
                Assert.NotNull(product.Description);
            }
        }

        [Fact]
        public async Task GetProductsWithReviewsAsync_ShouldReturnValidReviewData()
        {
            // Arrange
            var productService = new ProductService();

            // Act
            var result = await productService.GetProductsWithReviewsAsync();

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
        public async Task GetProductsWithReviewsAsync_ShouldReturnValidProductData()
        {
            // Arrange
            var productService = new ProductService();

            // Act
            var result = await productService.GetProductsWithReviewsAsync();

            // Assert
            foreach (var product in result)
            {
                Assert.NotNull(product.ProductId);
                Assert.NotNull(product.ProductNumber);
                Assert.InRange(product.SalePrice, 0, decimal.MaxValue);
            }
        }




        [Fact]
        public async Task GetProductsWithReviewsAsync_ShouldReturnNonEmptyReviewsList()
        {
            // Arrange
            var productService = new ProductService();

            // Act
            var result = await productService.GetProductsWithReviewsAsync();

            // Assert
            foreach (var product in result)
            {
                Assert.NotNull(product.Reviews);
                Assert.NotEmpty(product.Reviews);
            }
        }

        [Fact]
        public async Task GetProductsWithReviewsAsync_ShouldReturnNonNullProductFields()
        {
            // Arrange
            var productService = new ProductService();

            // Act
            var result = await productService.GetProductsWithReviewsAsync();

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
        public async Task GetProductsWithReviewsAsync_ShouldReturnValidPriceRange()
        {
            // Arrange
            var productService = new ProductService();

            // Act
            var result = await productService.GetProductsWithReviewsAsync();

            // Assert
            foreach (var product in result)
            {
                Assert.InRange(product.PriceExcTax, 0, decimal.MaxValue);
                Assert.InRange(product.PriceIncTax, 0, decimal.MaxValue);
                Assert.InRange(product.SalePrice, 0, decimal.MaxValue);
            }
        }



    }
}
