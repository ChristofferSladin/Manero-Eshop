using ManeroWebApp.Models;
using Newtonsoft.Json;
using ServiceLibrary.Services;
using System.Diagnostics;

namespace ManeroWebApp.Services
{
    public class ProductControllerService : IProductControllerService
    {
        private readonly IUserService _userService;
        private readonly IReviewService _reviewService;
        private readonly IProductService _productService;
        private readonly IShoppingCartService _shoppingCartService;
        private readonly IJwtAuthenticationService _authenticationService;

        public ProductControllerService(IReviewService reviewService, IProductService productService, IShoppingCartService shoppingCartService, IUserService userService, IJwtAuthenticationService authenticationService)
        {
            _reviewService = reviewService;
            _productService = productService;
            _shoppingCartService = shoppingCartService;
            _userService = userService;
            _authenticationService = authenticationService;
        }

        public async Task<bool> RefreshToken()
        {
            return await _authenticationService.RefreshTokenAsync();
        }
        public async Task<List<ShoppingCartViewModel>> GetShoppingForUserCartAsync()
        {
            var shoppingCart = new List<ShoppingCartViewModel>();
            try
            {

                var shoppingCartItems = await _shoppingCartService.GetUserShoppingCartProductsAsync();
                foreach (var cartItem in shoppingCartItems)
                {
                    var item = await _productService.GetProductByIdAsync(cartItem.ProductId);
                    ShoppingCartViewModel cartViewModel = item;
                    cartViewModel.ItemQuantity = cartItem.ItemQuantity;
                    shoppingCart.Add(cartViewModel);
                }

                return shoppingCart;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }

            return new List<ShoppingCartViewModel>();
        }
        public async Task<List<ShoppingCartViewModel>> GetShoppingForGuestCartAsync(string shoppingCartCookie)
        {
            var shoppingCart = new List<ShoppingCartViewModel>();
            try
            {

                var shoppingCartItems = JsonConvert.DeserializeObject<List<ShoppingCartItems>>(shoppingCartCookie);
                foreach (var cartItem in shoppingCartItems)
                {
                    var item = await _productService.GetProductAsync(cartItem.ProductNumber);
                    ShoppingCartViewModel cartViewModel = item;
                    cartViewModel.ItemQuantity = cartItem.ItemQuantity;
                    shoppingCart.Add(cartViewModel);
                }

                return shoppingCart;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }

            return new List<ShoppingCartViewModel>();
        }
        public async Task AddProductToShoppingCartForUserAsync(int itemQuantity, string productNumber)
        {
            await _shoppingCartService.AddProductToShoppingCartAsync(itemQuantity, productNumber);
        }
        public void AddProductToShoppingCartForGuest(HttpResponse response, string? existingShoppingCartCookie, int itemQuantity, string productNumber)
        {
            var shoppingCart = new ShoppingCartItems
            {
                ProductNumber = productNumber,
                ItemQuantity = itemQuantity
            };
            var cookieOptions = new CookieOptions { Expires = DateTime.Now.AddDays(30) };

            if (!string.IsNullOrEmpty(existingShoppingCartCookie))
            {
                var existingShoppingCart = JsonConvert.DeserializeObject<List<ShoppingCartItems>>(existingShoppingCartCookie);
                if (existingShoppingCart != null)
                {
                    var existingItem = existingShoppingCart.FirstOrDefault(item => item.ProductNumber == productNumber);

                    if (existingItem != null)
                    {
                        existingItem.ItemQuantity += itemQuantity;
                    }
                    else
                    {
                        existingShoppingCart.Add(shoppingCart);
                    }
                }

                response.Cookies.Append("ShoppingCart", JsonConvert.SerializeObject(existingShoppingCart), cookieOptions);
            }
            else
            {
                var newShoppingCart = new List<ShoppingCartItems> { shoppingCart };
                response.Cookies.Append("ShoppingCart", JsonConvert.SerializeObject(newShoppingCart), cookieOptions);
            }
        }

        public async Task<List<ProductViewModel>> GetProductsWithReviewsAsync()
        {
            var products = await _productService.GetProductsWithReviewsAsync();
            return products.Select(productViewModel => (ProductViewModel)productViewModel).ToList();
        }

        public async Task<ProductViewModel> GetProductAsync(string productNumber)
        {
            ProductViewModel productViewModel = await _productService.GetProductAsync(productNumber);
            return productViewModel;
        }

        public async Task<RatingViewModel> GetReviewDataAsync(string productName)
        {
            var reviews = await _reviewService.GetFilteredReviewsAsync(null, null, null, null, productName);
            var rating = 0.0M;
            var reviewCount = 0;
            foreach (var p in reviews)
            {
                rating += p.Rating;
                reviewCount++;
            }
            if (reviewCount != 0)
            {
                rating /= reviewCount;
            }
            var ratingViewModel = new RatingViewModel
            {
                Rating = rating,
                ReviewCount = reviewCount,
            };

            return ratingViewModel;
        }

        public async Task<List<SizeViewModel>> GetProductSizesAsync(string productName, string productNumber)
        {
            var products = await _productService.GetFilteredProductsAsync(null, null, null, "size", "asc", productName);
            var sizeViewModel = products.Select(p => new SizeViewModel
            {
                ProductName = p.ProductName,
                ProductNumber = p.ProductNumber!,
                Size = p.Size,
            }).ToList();

            var sizes = new[] { "XXS", "XS", "S", "M", "L", "X", "XL", "XXL", "XXXL", "XXXXL" };
            sizeViewModel = sizeViewModel
                .OrderBy(s => sizes.Contains(s.Size) ? "0" : "1")
                .ThenBy(s => Array.IndexOf(sizes, s.Size))
                .ThenBy(s => s.Size).GroupBy(c => c.Size)
                .Select(group => group.FirstOrDefault(c => c.ProductNumber == productNumber) ?? group.First())
                .ToList();

            return sizeViewModel;
        }

        public async Task<List<ColorViewModel>> GetProductColorsAsync(string productName, string size)
        {
            var products = await _productService.GetFilteredProductsAsync(null, null, null, "size", "asc", productName);
            var colorViewModel = products.Where(p => p.Size == size).Select(p => new ColorViewModel
            {
                ProductName = p.ProductName,
                ProductNumber = p.ProductNumber!,
                Color = p.Color,

            }).ToList();

            return colorViewModel;
        }

        public async Task<List<ReviewViewModel>> GetReviewsForProductAsync(string productName, int? take)
        {
            var reviews = await _reviewService.GetFilteredReviewsAsync(null, take, "created", "desc", productName);

            var reviewsViewModel = reviews.Select(r =>
            {
                var userProfile = _userService.GetUserProfileAsync(r.Id).Result;
                return new ReviewViewModel
                {
                    ReviewId = r.ReviewId,
                    ProductName = r.ProductName,
                    Rating = r.Rating,
                    Created = r.Created,
                    Content = r.Content,
                    Title = r.Title,
                    ProductId = r.ProductId,
                    Id = r.Id,

                    FirstName = userProfile.FirstName,
                    LastName = userProfile.LastName,
                    ProfileImage = userProfile.ProfileImage,
                };
            }).ToList();

            return reviewsViewModel;
        }
    }
}
