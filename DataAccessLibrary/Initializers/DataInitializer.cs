
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
        SeedUserProfiles();
        SeedProducts();
        SeedShoppingCarts();
        SeedOrders();
        SeedFavoriteProducts();
        SeedReviews();
        SeedCards();
        SeedAddresses();
    }
    private void SeedUserProfiles()
    {
        AddUserProfileIfNotExist("Cust", "Omer1", "customer1@customer.com", "https://images.unsplash.com/photo-1438761681033-6461ffad8d80?q=80&w=2670&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D");
        AddUserProfileIfNotExist("Cust", "Omer2", "customer2@customer.com", "https://images.unsplash.com/photo-1547425260-76bcadfb4f2c?q=80&w=2670&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D");
        AddUserProfileIfNotExist("Cust", "Omer3", "customer3@customer.com", "https://images.unsplash.com/photo-1552058544-f2b08422138a?q=80&w=2598&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D");
        AddUserProfileIfNotExist("Cust", "Omer4", "customer4@customer.com", "https://images.unsplash.com/photo-1499952127939-9bbf5af6c51c?q=80&w=2676&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D");
        AddUserProfileIfNotExist("Cust", "Omer5", "customer5@customer.com", "https://images.unsplash.com/photo-1599566150163-29194dcaad36?q=80&w=2574&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D");
    }

    public void AddUserProfileIfNotExist(string firstName, string lastName, string userEmail, string userProfileImageUrl)
    {
        var userExists = _context.Users.FirstOrDefault(u => u.Email == userEmail);
        if (userExists != null)
        {
            var userProfile = _context.UserProfiles.FirstOrDefault(o => o.Id == userExists.Id);
            if (userProfile == null)
            {
                _context.UserProfiles.Add(new UserProfile
                {
                    ProfileImage = userProfileImageUrl,
                    Id = userExists.Id,
                    FirstName = firstName,
                    LastName = lastName
                });
                _context.SaveChanges();
            }
        }
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
    private void AddUserIfNotExists(string email, string password, string[] roles)
    {
        if (_userManager.FindByEmailAsync(email).Result != null) return;

        var user = new IdentityUser
        {
            UserName = email,
            Email = email,
            EmailConfirmed = true
        };
        _userManager.CreateAsync(user, password).Wait();
        _userManager.AddToRolesAsync(user, roles).Wait();
        _context.SaveChanges();
    }

    private void SeedRoles()
    {
        AddRoleIfNotExisting("None");
        AddRoleIfNotExisting("Admin");
        AddRoleIfNotExisting("Customer");
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

    private void SeedShoppingCarts()
    {
        AddShoppingCartIfNotExisting();
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
                        TotalPriceIncTax = _context.Products.FirstOrDefault(p => p.ProductName.ToLower() == "Denim Jacket".ToLower())!.PriceIncTax * 2,
                        TotalPriceExcTax = _context.Products.FirstOrDefault(p => p.ProductName.ToLower() == "Denim Jacket".ToLower())!.PriceExcTax * 2,
                        ShoppingCartId = shoppingCartExists.ShoppingCartId,
                    };

                    var shoppingCartProducts2 = new ShoppingCartProduct
                    {
                        ItemQuantity = 10,
                        Product = _context.Products.FirstOrDefault(p => p.ProductName.ToLower() == "Cargo Pants".ToLower())!,
                        TotalPriceIncTax = _context.Products.FirstOrDefault(p => p.ProductName.ToLower() == "Cargo Pants".ToLower())!.PriceExcTax * 10,
                        TotalPriceExcTax = _context.Products.FirstOrDefault(p => p.ProductName.ToLower() == "Cargo Pants".ToLower())!.PriceIncTax * 10,
                        ShoppingCartId = shoppingCartExists.ShoppingCartId,
                    };
                    _context.AddRange(shoppingCartProducts1, shoppingCartProducts2);
                    _context.SaveChanges();
                }
            }
        }
    }
    private void SeedOrders()
    {
        AddOrdersIfNotExisting();
    }
    private void AddOrdersIfNotExisting()
    {
        var userExists = _context.Users.FirstOrDefault(u => u.Email == "customer1@customer.com");
        if (userExists != null)
        {
            var orderExists = _context.Orders.FirstOrDefault(o => o.Id == userExists.Id);
            if (orderExists == null)
            {
                var payment = _context.Add(new Payment
                {
                    PaymentMethod = PaymentMethod.Swish
                });

                var totalPriceExcTax = _context.Products.FirstOrDefault(p => p.ProductName.ToLower() == "Cargo Pants".ToLower())!.PriceExcTax * 10 + _context.Products.FirstOrDefault(p => p.ProductName.ToLower() == "Denim Jacket".ToLower())!.PriceExcTax * 2;
                var totalPriceIncTax = _context.Products.FirstOrDefault(p => p.ProductName.ToLower() == "Cargo Pants".ToLower())!.PriceIncTax * 10 + _context.Products.FirstOrDefault(p => p.ProductName.ToLower() == "Denim Jacket".ToLower())!.PriceIncTax * 2;

                var order = _context.Add(new Order
                {
                    OrderStatus = OrderStatus.InProcess,
                    Payment = payment.Entity,
                    TotalPriceExcTax = totalPriceExcTax,
                    TotalPriceIncTax = totalPriceIncTax,
                    TaxPercentage = 0.2M,
                    VatTax = 0.0M,
                    OrderDate = DateTime.Now,
                    PaymentDate = DateTime.Now.AddDays(1),
                    Id = userExists.Id,
                });
                _context.SaveChanges();

                if (order != null!)
                {
                    order.Entity.GenerateOrderNumber();
                    _context.Update(order.Entity);
                    _context.SaveChanges();
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
                        OrderId = orderExists.OrderId
                    };

                    var orderProduct2 = new OrderProduct
                    {
                        ItemQuantity = 10,
                        Product = _context.Products.FirstOrDefault(p => p.ProductName.ToLower() == "Cargo Pants".ToLower())!,
                        OrderId = orderExists.OrderId
                    };
                    _context.OrderProducts.AddRange(orderProduct1, orderProduct2);
                    _context.SaveChanges();
                }
            }
        }
    }
    private void SeedFavoriteProducts()
    {
        AddFavoriteProductsIfNotExisting();
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
    private void SeedCards()
    {
        var cardsList = new List<Card>
        {
            new() { CardNumber = "8888 9999 4565 7787",CardHolderName = "Chris", ExpirationDate = DateTime.Now.AddMonths(12),CardType = "MasterCard", IssuerBank = "SEB" },
            new() { CardNumber = "2322 4322 4222 5567",CardHolderName = "Erim", ExpirationDate = DateTime.Now.AddMonths(24),CardType = "VisaCard",IssuerBank = "Handelsbanken" },
            new() { CardNumber = "8888 9334 4424 7227",CardHolderName = "Ghazanfar", ExpirationDate = DateTime.Now.AddMonths(36),CardType = "Maestro",IssuerBank = "Nordea" },
            new() { CardNumber  = "2812 9999 4565 7787",CardHolderName = "Hadi", ExpirationDate = DateTime.Now.AddMonths(18),CardType = "VisaCard",IssuerBank = "SEB" },
            new() { CardNumber  = "1188 3349 4265 3237",CardHolderName = "Jonathan",ExpirationDate = DateTime.Now.AddMonths(20),CardType = "MasterCard", IssuerBank = "Swedia" }
        };

        AddCardIfNotExisting(cardsList);
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
    private void SeedProducts()
    {
        AddProductIfNotExisting(15, "Blue", "Denim Jacket", "Perfect for casual outings", "Jackets", "Casual", "S", 69.99M, (69.99M * 1.2M), 0.8M, true, false, 4.5M, "https://images.unsplash.com/photo-1615943168243-5b2503679e47?q=80&w=2565&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D");
        AddProductIfNotExisting(15, "Blue", "Denim Jacket", "Perfect for casual outings", "Jackets", "Casual", "M", 69.99M, (69.99M * 1.2M), 0.8M, true, false, 4.5M, "https://images.unsplash.com/photo-1615943168243-5b2503679e47?q=80&w=2565&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D");
        AddProductIfNotExisting(15, "Red", "Denim Jacket", "Perfect for casual outings", "Jackets", "Casual", "M", 69.99M, (69.99M * 1.2M), 0.8M, true, false, 4.5M, "https://images.unsplash.com/photo-1615943168243-5b2503679e47?q=80&w=2565&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D");
        AddProductIfNotExisting(15, "Black", "Denim Jacket", "Perfect for casual outings", "Jackets", "Casual", "M", 69.99M, (69.99M * 1.2M), 0.8M, true, false, 4.5M, "https://images.unsplash.com/photo-1615943168243-5b2503679e47?q=80&w=2565&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D");
        AddProductIfNotExisting(15, "Yellow", "Denim Jacket", "Perfect for casual outings", "Jackets", "Casual", "M", 69.99M, (69.99M * 1.2M), 0.8M, true, false, 4.5M, "https://images.unsplash.com/photo-1615943168243-5b2503679e47?q=80&w=2565&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D");
        AddProductIfNotExisting(15, "Blue", "Denim Jacket", "Perfect for casual outings", "Jackets", "Casual", "L", 69.99M, (69.99M * 1.2M), 0.8M, true, false, 4.5M, "https://images.unsplash.com/photo-1615943168243-5b2503679e47?q=80&w=2565&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D");
        AddProductIfNotExisting(15, "Red", "Denim Jacket", "Perfect for casual outings", "Jackets", "Casual", "L", 69.99M, (69.99M * 1.2M), 0.8M, true, false, 4.5M, "https://images.unsplash.com/photo-1615943168243-5b2503679e47?q=80&w=2565&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D");
        AddProductIfNotExisting(15, "Blue", "Denim Jacket", "Perfect for casual outings", "Jackets", "Casual", "XL", 69.99M, (69.99M * 1.2M), 0.8M, true, false, 4.5M, "https://images.unsplash.com/photo-1615943168243-5b2503679e47?q=80&w=2565&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D");

        AddProductIfNotExisting(13, "Black", "Leather Jacket", "Stylish and warm", "Jackets", "Formal", "L", 129.99M, (129.99M * 1.2M), 0.8M, true, true, 5.0M, "https://images.unsplash.com/photo-1557684387-08927d28c72a?q=80&w=2576&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D");
        AddProductIfNotExisting(33, "White", "T-Shirt", "Comfortable cotton t-shirt", "T-Shirts", "Casual", "M", 19.99M, (19.99M * 1.2M), 0.8M, false, false, 4.0M, "https://images.unsplash.com/photo-1586790170083-2f9ceadc732d?q=80&w=2574&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D");
        AddProductIfNotExisting(10, "Green", "Cargo Pants", "Roomy and comfortable", "Pants", "Casual", "L", 49.99M, (49.99M * 1.2M), 0.95M, true, false, 4.3M, "https://images.unsplash.com/photo-1473966968600-fa801b869a1a?q=80&w=2000&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D");
        AddProductIfNotExisting(43, "Black", "Formal Shoes", "Perfect for business attire", "Shoes", "Formal", "10", 89.99M, (89.99M * 1.2M), 0.8M, false, true, 4.7M, "https://images.unsplash.com/photo-1609259886986-a642e7e1dbf9?w=600&auto=format&fit=crop&q=60&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxzZWFyY2h8MTB8fGZvcm1hbCUyMHNob2VzfGVufDB8fDB8fHww");
        AddProductIfNotExisting(77, "Red", "Sneaky Shoes", "Perfect for robbing attire", "Shoes", "Formal", "16", 89.99M, (89.99M * 1.2M), 0.9M, false, true, 5.0M, "https://images.unsplash.com/photo-1529925692078-9eca152749cd?q=80&w=2670&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D");
    }
    private void AddProductIfNotExisting(int quantity, string color, string productName, string? description, string? category, string? type, string? size, decimal priceExcTax, decimal priceIncTax, decimal salePricePercentage, bool isOnSale, bool isFeatured, decimal rating, string? imageUrl)
    {
        EntityEntry<Product> addedProduct = null!;

        if (!_context.Products.Any(p => p.ProductName == productName && p.Size == size && p.Color == color))
        {
            var product = new Product
            {
                ProductName = productName,
                Description = description,
                Category = category,
                Color = color,
                Type = type,
                Size = size,
                PriceExcTax = priceExcTax,
                PriceIncTax = priceIncTax,
                QuantityInStock = quantity,
                SalePricePercentage = salePricePercentage,
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
}
