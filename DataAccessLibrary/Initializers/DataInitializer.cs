
using DataAccessLibrary.Contexts;
using DataAccessLibrary.Entities.ProductEntities;
using DataAccessLibrary.Entities.UserEntities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;

namespace DataAccessLibrary.Initializers;

public class DataInitializer
{
    private readonly ManeroDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;

    public DataInitializer(ManeroDbContext context, UserManager<IdentityUser> userManager)
    {
        _context = context;
        _userManager = userManager;

    }
    public void SeedData()
    {
        SeedRoles();
        SeedUsers();
        SeedProducts();
        SeedShoppingCarts();
        SeedOrders();
        SeedFavoriteProducts();
        SeedReviews();
        SeedCards();
        SeedAddresses();
    }
    private void SeedUsers()
    {
        AddUserIfNotExists("admin@admin.com", "Admin123#", new string[] { "Admin" });
        AddUserIfNotExists("customer1@customer.com", "Customer123#", new string[] { "Customer" });
        AddUserIfNotExists("customer2@customer.com", "Customer123#", new string[] { "Customer" });
        AddUserIfNotExists("customer3@customer.com", "Customer123#", new string[] { "Customer" });
        AddUserIfNotExists("customer4@customer.com", "Customer123#", new string[] { "Customer" });
        AddUserIfNotExists("customer5@customer.com", "Customer123#", new string[] { "Customer" });
    }
    private void SeedRoles()
    {
        AddRoleIfNotExisting("None");
        AddRoleIfNotExisting("Admin");
        AddRoleIfNotExisting("Customer");
    }
    private void SeedProducts()
    {
        AddProductIfNotExisting(15, "Blue", "Denim Jacket", "Perfect for casual outings", "Jackets", "Casual", "M", 69.99M, 59.99M, true, false, 4.5M, "denim_jacket_url");
        AddProductIfNotExisting(13, "Black", "Leather Jacket", "Stylish and warm", "Jackets", "Formal", "L", 129.99M, 99.99M, true, true, 5.0M, "leather_jacket_url");
        AddProductIfNotExisting(33, "White", "T-Shirt", "Comfortable cotton t-shirt", "T-Shirts", "Casual", "M", 19.99M, 14.99M, false, false, 4.0M, "tshirt_url");
        AddProductIfNotExisting(10, "Green", "Cargo Pants", "Roomy and comfortable", "Pants", "Casual", "L", 49.99M, 39.99M, true, false, 4.3M, "cargo_pants_url");
        AddProductIfNotExisting(43, "Black", "Formal Shoes", "Perfect for business attire", "Shoes", "Formal", "10", 89.99M, 79.99M, false, true, 4.7M, "formal_shoes_url");
        AddProductIfNotExisting(77, "Red", "Sneaky Shoes", "Perfect for robbing attire", "Shoes", "Formal", "16", 89.99M, 79.99M, false, true, 5.0M, "sneaky_robbing_shoes_url");
    }
    private void SeedShoppingCarts()
    {
        AddShoppingCartIfNotExisting();
    }
    private void SeedOrders()
    {
        AddOrdersIfNotExisting();
    }
    private void SeedFavoriteProducts()
    {
        AddFavoriteProductsIfNotExisting();
    }
    private void SeedAddresses()
    {
        var addresses = new List<Address>
        {
            new() { StreetName = "CicoBolo Street 69", City = "Las Vegas", ZipCode = "9000 69", Country = "USA" },
            new() { StreetName = "La calle Berox 88", City = "Miami", ZipCode = "800052", Country = "USA" },
        };
        var users = _context.Users.Take(2).ToList();
        AddAddressIfNotExists(users, addresses);
    }
    private void SeedCards()
    {
        var cardsList = new List<Card>
        {
            new() { CardNumber = "8888 9999 4565 7787",CardHolderName = "Chris", ExpirationDate = DateTime.Now.AddMonths(12),SecurityCode = "889",CardType = "MasterCard", IssuerBank = "SEB" },
            new() { CardNumber = "2322 4322 4222 5567",CardHolderName = "Erim", ExpirationDate = DateTime.Now.AddMonths(24),SecurityCode = "823",CardType = "VisaCard",IssuerBank = "Handelsbanken" },
            new() { CardNumber = "8888 9334 4424 7227",CardHolderName = "Ghazanfar", ExpirationDate = DateTime.Now.AddMonths(36),SecurityCode = "189",CardType = "Maestro",IssuerBank = "Nordea" },
            new() { CardNumber  = "2812 9999 4565 7787",CardHolderName = "Hadi", ExpirationDate = DateTime.Now.AddMonths(18), SecurityCode = "283",CardType = "VisaCard",IssuerBank = "SEB" },
            new() { CardNumber  = "1188 3349 4265 3237",CardHolderName = "Jonathan",ExpirationDate = DateTime.Now.AddMonths(20),SecurityCode = "419",CardType = "MasterCard", IssuerBank = "Swedia" }
        };

        AddCardIfNotExisting(cardsList);
    }
    private void SeedReviews()
    {
        var users = _context.Users.ToList();
        var products = _context.Products.ToList();

        foreach (var user in users)
        {
            //checks for user that has role "customer".
            var hasRoleCustomer = _context.UserRoles
                .Where(ur => ur.UserId == user.Id)
                .Join(_context.Roles, ur => ur.RoleId, r => r.Id, (ur, r) => r)
                .FirstOrDefault(r => r.Name != null && r.Name.ToLower() == "customer")?.Name;

            if (hasRoleCustomer != null)
            {
                //randomizes a product to review
                var rand = new Random();
                rand.Next(products.Count);
                var randomProduct = products[rand.Next(products.Count)];
                AddReviewsIfNotExisting(user, randomProduct);
            }
        }
    }



    private void AddCardIfNotExisting(List<Card> cardsList)
    {
        var usersList = _context.Users.ToList();

        foreach (var user in usersList)
        {
            var hasRoleCustomer = _context.UserRoles
                .Where(ur => ur.UserId == user.Id)
                .Join(_context.Roles, ur => ur.RoleId, r => r.Id, (ur, r) => r)
                .FirstOrDefault(r => r.Name != null && r.Name.ToLower() == "customer")?.Name;

            if (hasRoleCustomer is not null)
            {
                var random = new Random();
                var card = cardsList[random.Next(cardsList.Count)];

                card.Id = user.Id;

                var userAlreadyHasCard = _context.Cards.FirstOrDefault(x => x.Id == user.Id);

                if (userAlreadyHasCard is null)
                {
                    _context.Cards.Add(card);
                    _context.SaveChanges();
                    cardsList.Remove(card);
                }
            }
        }
    }
    private void AddReviewsIfNotExisting(IdentityUser user, Product product)
    {
        var rand = new Random();
        var rating = rand.Next(0, 6);

        var review = new Review
        {
            Rating = rating,
            Created = DateTime.Now,
            Title = "Lorem Ipsum",
            Content = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ullamcorper a lacus vestibulum sed.",
            ProductId = product.ProductId,
            Id = user.Id,
        };
        var userAlreadyHasReview = _context.Reviews.FirstOrDefault(r => r.Id == user.Id);
        //checks if user already has review to not create one if one exists.
        if (userAlreadyHasReview == null)
        {
            _context.Add(review);
            _context.SaveChanges();
        }
    }
    private void AddAddressIfNotExists(List<IdentityUser> users, List<Address> addresses)
    {
        foreach (var user in users)
        {
            var existingAddress = _context.Addresses.FirstOrDefault(a => a.Id == user.Id);
            if (existingAddress == null)
            {
                var rand = new Random();
                var address = addresses[rand.Next(addresses.Count)];
                address.Id = user.Id;

                _context.Addresses.Add(address);
                _context.SaveChanges();
                addresses.Remove(address);
            }
        }
    }
    private void AddFavoriteProductsIfNotExisting()
    {
        var userExists = _context.Users.FirstOrDefault(u => u.Email == "customer1@customer.com");
        if (userExists != null)
        {
            var favoriteProductsExists = _context.Favorite.Include(o => o.FavoriteProducts).FirstOrDefault(o => o.Id == userExists.Id);
            if (favoriteProductsExists == null)
            {
                var favoriteProducts = new Favorite
                {
                    Id = userExists.Id,
                };
                _context.Add(favoriteProducts);
                _context.SaveChanges();
            }

            favoriteProductsExists = _context.Favorite.Include(o => o.FavoriteProducts).FirstOrDefault(o => o.Id == userExists.Id);
            if (favoriteProductsExists != null)
            {
                if (favoriteProductsExists.FavoriteProducts != null && !favoriteProductsExists.FavoriteProducts.Any())
                {
                    var favoriteProduct1 = new FavoriteProduct
                    {
                        ProductId = _context.Products.Skip(0).Take(1).FirstOrDefault()!.ProductId,
                        FavoriteId = favoriteProductsExists.FavoriteId
                    };
                    var favoriteProduct2 = new FavoriteProduct
                    {
                        ProductId = _context.Products.Skip(1).Take(1).FirstOrDefault()!.ProductId,
                        FavoriteId = favoriteProductsExists.FavoriteId
                    };
                    var favoriteProduct3 = new FavoriteProduct
                    {
                        ProductId = _context.Products.Skip(2).Take(1).FirstOrDefault()!.ProductId,
                        FavoriteId = favoriteProductsExists.FavoriteId
                    };
                    var favoriteProduct4 = new FavoriteProduct
                    {
                        ProductId = _context.Products.Skip(3).Take(1).FirstOrDefault()!.ProductId,
                        FavoriteId = favoriteProductsExists.FavoriteId
                    };
                    _context.AddRange(favoriteProduct1, favoriteProduct2, favoriteProduct3, favoriteProduct4);
                    _context.SaveChanges();
                }
            }
        }
    }
    private void AddOrdersIfNotExisting()
    {
        var userExists = _context.Users.FirstOrDefault(u => u.Email == "customer1@customer.com");
        if (userExists != null)
        {
            var orderExists = _context.Orders.FirstOrDefault(o => o.Id == userExists.Id);
            if (orderExists == null)
            {
                var order = new Order
                {
                    OrderStatus = OrderStatus.InProcess,
                    PaymentMethod = PaymentMethod.Card,
                    TotalAmount = _context.Products.FirstOrDefault(p => p.ProductName.ToLower() == "Cargo Pants".ToLower())!.Price * 10 + _context.Products.FirstOrDefault(p => p.ProductName.ToLower() == "Denim Jacket".ToLower())!.Price * 2,
                    Created = DateTime.Now,
                    Id = userExists.Id,
                };
                var createdOrder = _context.Add(order);
                _context.SaveChanges();

                if (createdOrder != null!)
                {
                    createdOrder.Entity.GenerateOrderNumber();
                    _context.Update(createdOrder.Entity);
                }
            }

            orderExists = _context.Orders.Include(o => o.OrderProducts).FirstOrDefault(o => o.Id == userExists.Id);
            if (orderExists != null)
            {
                if (!orderExists.OrderProducts.Any())
                {
                    var orderProduct1 = new OrderProduct
                    {
                        ItemQuantity = 2,
                        Product = _context.Products.FirstOrDefault(p => p.ProductName.ToLower() == "Denim Jacket".ToLower())!,
                        TotalPrice = _context.Products.FirstOrDefault(p => p.ProductName.ToLower() == "Denim Jacket".ToLower())!.Price * 2,
                        OrderId = orderExists.OrderId
                    };

                    var orderProduct2 = new OrderProduct
                    {
                        ItemQuantity = 10,
                        Product = _context.Products.FirstOrDefault(p => p.ProductName.ToLower() == "Cargo Pants".ToLower())!,
                        TotalPrice = _context.Products.FirstOrDefault(p => p.ProductName.ToLower() == "Cargo Pants".ToLower())!.Price * 10,
                        OrderId = orderExists.OrderId
                    };
                    _context.OrderProducts.AddRange(orderProduct1, orderProduct2);
                    _context.SaveChanges();
                }
            }
        }
    }
    private void AddShoppingCartIfNotExisting()
    {
        var userExists = _context.Users.FirstOrDefault(u => u.Email == "customer1@customer.com");
        if (userExists != null)
        {
            var shoppingCartExists = _context.ShoppingCarts.FirstOrDefault(o => o.Id == userExists.Id);
            if (shoppingCartExists == null)
            {
                var shoppingCart = new ShoppingCart
                {
                    Id = userExists.Id
                };
                _context.Add(shoppingCart);
                _context.SaveChanges();
            }

            shoppingCartExists = _context.ShoppingCarts.Include(s => s.ShoppingCartProducts).FirstOrDefault(o => o.Id == userExists.Id);
            if (shoppingCartExists != null)
            {
                if (shoppingCartExists.ShoppingCartProducts != null && !shoppingCartExists.ShoppingCartProducts.Any())
                {
                    var shoppingCartProducts1 = new ShoppingCartProduct
                    {
                        ItemQuantity = 2,
                        Product = _context.Products.FirstOrDefault(p => p.ProductName.ToLower() == "Denim Jacket".ToLower())!,
                        TotalPrice = _context.Products.FirstOrDefault(p => p.ProductName.ToLower() == "Denim Jacket".ToLower())!.Price * 2,
                        ShoppingCartId = shoppingCartExists.ShoppingCartId,
                    };

                    var shoppingCartProducts2 = new ShoppingCartProduct
                    {
                        ItemQuantity = 10,
                        Product = _context.Products.FirstOrDefault(p => p.ProductName.ToLower() == "Cargo Pants".ToLower())!,
                        TotalPrice = _context.Products.FirstOrDefault(p => p.ProductName.ToLower() == "Cargo Pants".ToLower())!.Price * 10,
                        ShoppingCartId = shoppingCartExists.ShoppingCartId,
                    };
                    _context.AddRange(shoppingCartProducts1, shoppingCartProducts2);
                    _context.SaveChanges();
                }
            }
        }
    }
    private void AddProductIfNotExisting(int quantity, string color, string productName, string? description, string? category, string? type, string? size, decimal price, decimal salePrice, bool isOnSale, bool isFeatured, decimal rating, string? imageUrl)
    {
        EntityEntry<Product> addedProduct = null!;

        if (!_context.Products.Any(p => p.ProductName == productName))
        {
            var product = new Product
            {
                ProductName = productName,
                Description = description,
                Category = category,
                Color = color,
                Type = type,
                Size = size,
                Price = price,
                QuantityInStock = quantity,
                SalePrice = salePrice,
                IsOnSale = isOnSale,
                IsFeatured = isFeatured,
                Rating = rating,
                ImageUrl = imageUrl,
            };
            addedProduct = _context.Products.Add(product);
            _context.SaveChanges();
        }

        if (addedProduct != null)
        {
            addedProduct.Entity.GenerateProductNumber();
            _context.Update(addedProduct.Entity);
        }
        _context.SaveChanges();

    }
    private void AddUserIfNotExists(string userName, string password, string[] roles)
    {
        if (_userManager.FindByEmailAsync(userName).Result != null) return;

        var user = new IdentityUser
        {
            UserName = userName,
            Email = userName,
            EmailConfirmed = true
        };
        _userManager.CreateAsync(user, password).Wait();
        _userManager.AddToRolesAsync(user, roles).Wait();
    }
    private void AddRoleIfNotExisting(string roleName)
    {
        var role = _context.Roles.FirstOrDefault(r => r.Name == roleName);
        if (role == null)
        {
            _context.Roles.Add(new IdentityRole { Name = roleName, NormalizedName = roleName });
            _context.SaveChanges();
        }
    }
}
