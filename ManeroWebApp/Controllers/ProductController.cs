﻿using System.ComponentModel.DataAnnotations;
using ManeroWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using ServiceLibrary.Models;
using ServiceLibrary.Services;

namespace ManeroWebApp.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }
        public async Task<IActionResult> Products()
        {
            var products = await _productService.GetProductsWithReviewsAsync();
            var productList = products.Select(product => new ProductViewModel
            {
                ProductId = product.ProductId,
                ProductNumber = product.ProductNumber,
                ProductName = product.ProductName,
                Description = product.Description,
                Category = product.Category,
                Type = product.Type,
                Size = product.Size,
                QuantityInStock = product.QuantityInStock,
                Color = product.Color,
                PriceExcTax = product.PriceExcTax,
                PriceIncTax = product.PriceIncTax,
                SalePrice = product.SalePrice,
                IsOnSale = product.IsOnSale,
                IsFeatured = product.IsFeatured,
                Rating = product.Rating,
                ImageUrl = product.ImageUrl
            }).ToList();

            return View(productList);
        }
    }
}
