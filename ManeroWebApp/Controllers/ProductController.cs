﻿using ManeroWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using ServiceLibrary.ProductServices;

namespace ManeroWebApp.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

      
        public IActionResult FeaturedProduct()
        {
            var productList = new List<ProductViewModel>();
            var products = _productService.GetProductsAsync();

            foreach(var product in products.Result)
            {
                productList.Add(new ProductViewModel
                {
                    ProductId = product.ProductId,
                    ProductName = product.ProductName,

                 });
            }
            return View(productList);
        }

    }
}
