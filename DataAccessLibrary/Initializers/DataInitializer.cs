
using DataAccessLibrary.Contexts;
using DataAccessLibrary.Entities.OrderEntities;
using DataAccessLibrary.Entities.ProductEntities;
using DataAccessLibrary.Entities.UserEntities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Drawing;
using System.Security.Cryptography;

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
        SeedUserTokens();
        SeedUserProfiles();
        SeedProductCategories();
        SeedProducts();
        SeedShoppingCarts();
        SeedOrders();
        SeedFavoriteProducts();
        SeedCards();
        SeedAddresses();
        SeedReviews();
        SeedPromoCodes();
    }

    private void SeedUserTokens()
    {
        AddUserTokenIfNotExist();
    }

    private void AddUserTokenIfNotExist()
    {
        var users = _context.Users.ToList();
        foreach (var user in users)
        {
            if (!_context.UserRefreshToken.Any(x => x.Id == user.Id))
            {
                _context.Add(new UserRefreshToken
                {
                    Id = user.Id,
                    RefreshToken = null,
                    RefreshTokenExpiry = DateTime.UtcNow
                });
            }

        }

        _context.SaveChanges();
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
                    var product1 = _context.Products.FirstOrDefault(p => p.ProductName.ToLower() == "Denim Jacket".ToLower());
                    var product2 = _context.Products.FirstOrDefault(p => p.ProductName.ToLower() == "Cargo Pants".ToLower());
                    var orderProduct1 = new OrderProduct
                    {
                        ItemQuantity = 2,
                        ProductNumber = product1.ProductNumber,
                        ProductName = product1.ProductName,
                        PriceIncTax = product1.PriceIncTax,
                        PriceExcTax = product1.PriceExcTax,
                        SalePricePercentage = product1.SalePricePercentage,
                        OrderId = orderExists.OrderId
                    };

                    var orderProduct2 = new OrderProduct
                    {
                        ItemQuantity = 10,
                        ProductNumber = product2.ProductNumber,
                        ProductName = product2.ProductName,
                        PriceIncTax = product2.PriceIncTax,
                        PriceExcTax = product2.PriceExcTax,
                        SalePricePercentage = product2.SalePricePercentage,
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
            ProductName = product.ProductName,
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
    private void SeedProductCategories()
    {
        AddProductCategoriesIfNotExisting("Accessories", "https://images.unsplash.com/photo-1443884590026-2e4d21aee71c?q=80&w=2043&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D");
        AddProductCategoriesIfNotExisting("Coats", "https://images.unsplash.com/photo-1459163855801-ead7f9cdfcb7?q=80&w=2070&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D");
        AddProductCategoriesIfNotExisting("Jackets", "https://images.unsplash.com/photo-1559551409-dadc959f76b8?q=80&w=2073&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D");
        AddProductCategoriesIfNotExisting("Pants", "https://images.unsplash.com/photo-1542272604-787c3835535d?q=80&w=1926&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D");
        AddProductCategoriesIfNotExisting("Shirts", "https://images.unsplash.com/photo-1626497764746-6dc36546b388?q=80&w=1926&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D");
        AddProductCategoriesIfNotExisting("Shoes", "https://images.unsplash.com/photo-1542291026-7eec264c27ff?q=80&w=2070&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D");
        AddProductCategoriesIfNotExisting("Suits", "https://images.unsplash.com/photo-1551201602-3f9456f0fbf8?q=80&w=1974&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D");
        AddProductCategoriesIfNotExisting("Sweaters", "https://images.unsplash.com/photo-1599703678443-4fdafa9e1d0a?q=80&w=2025&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D");
        AddProductCategoriesIfNotExisting("Sweatshirts", "https://images.unsplash.com/photo-1566778938552-2af3eb48016d?q=80&w=2070&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D");
        AddProductCategoriesIfNotExisting("T-Shirts", "https://images.unsplash.com/photo-1523381294911-8d3cead13475?q=80&w=2070&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D");
        AddProductCategoriesIfNotExisting("Skirts", "https://images.unsplash.com/photo-1577900232427-18219b9166a0?q=80&w=2070&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D");
    }
    private void AddProductCategoriesIfNotExisting(string categoryName, string ImgUrl)
    {
        if (!_context.Categories.Any(c => c.CategoryName == categoryName))
        {
            var category = new Category
            {
                CategoryName = categoryName,
                ImgUrl = ImgUrl,
            };
            _context.Categories.Add(category);
            _context.SaveChanges();
        }
    }
    private void SeedProducts()
    {
        AddProductIfNotExisting(15, "Blue", "Denim Jacket", "Perfect for casual outings", "Jackets", "Casual", "S", 69.99M, (69.99M * 1.2M), 0.8M, true, false, 4.5M, "https://images.unsplash.com/photo-1615943168243-5b2503679e47?q=80&w=2565&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D", "Women");
        AddProductIfNotExisting(15, "Blue", "Denim Jacket", "Perfect for casual outings", "Jackets", "Casual", "M", 69.99M, (69.99M * 1.2M), 0.8M, true, false, 4.5M, "https://images.unsplash.com/photo-1615943168243-5b2503679e47?q=80&w=2565&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D", "Men");
        AddProductIfNotExisting(15, "Red", "Denim Jacket", "Perfect for casual outings", "Jackets", "Casual", "M", 69.99M, (69.99M * 1.2M), 0.8M, true, false, 4.5M, "https://images.unsplash.com/photo-1615943168243-5b2503679e47?q=80&w=2565&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D", "Women");
        AddProductIfNotExisting(15, "Black", "Denim Jacket", "Perfect for casual outings", "Jackets", "Casual", "M", 69.99M, (69.99M * 1.2M), 0.8M, true, false, 4.5M, "https://images.unsplash.com/photo-1615943168243-5b2503679e47?q=80&w=2565&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D", "Men");
        AddProductIfNotExisting(15, "Yellow", "Denim Jacket", "Perfect for casual outings", "Jackets", "Casual", "M", 69.99M, (69.99M * 1.2M), 0.8M, true, false, 4.5M, "https://images.unsplash.com/photo-1615943168243-5b2503679e47?q=80&w=2565&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D", "Girls");
        AddProductIfNotExisting(15, "Blue", "Denim Jacket", "Perfect for casual outings", "Jackets", "Casual", "L", 69.99M, (69.99M * 1.2M), 0.8M, true, false, 4.5M, "https://images.unsplash.com/photo-1615943168243-5b2503679e47?q=80&w=2565&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D", "Women");
        AddProductIfNotExisting(15, "Red", "Denim Jacket", "Perfect for casual outings", "Jackets", "Casual", "L", 69.99M, (69.99M * 1.2M), 0.8M, true, false, 4.5M, "https://images.unsplash.com/photo-1615943168243-5b2503679e47?q=80&w=2565&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D", "Men");
        AddProductIfNotExisting(15, "Blue", "Denim Jacket", "Perfect for casual outings", "Jackets", "Casual", "XL", 69.99M, (69.99M * 1.2M), 0.8M, true, false, 4.5M, "https://images.unsplash.com/photo-1615943168243-5b2503679e47?q=80&w=2565&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D", "Boys");

        AddProductIfNotExisting(13, "Black", "Leather Jacket", "Stylish and warm", "Jackets", "Formal", "L", 129.99M, (129.99M * 1.2M), 0.8M, true, true, 5.0M, "https://images.unsplash.com/photo-1557684387-08927d28c72a?q=80&w=2576&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D", "Women");
        AddProductIfNotExisting(33, "White", "T-Shirt", "Comfortable cotton t-shirt", "T-Shirts", "Casual", "M", 19.99M, (19.99M * 1.2M), 0.8M, false, false, 4.0M, "https://images.unsplash.com/photo-1586790170083-2f9ceadc732d?q=80&w=2574&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D", "Men");
        AddProductIfNotExisting(10, "Green", "Cargo Pants", "Roomy and comfortable", "Pants", "Casual", "L", 49.99M, (49.99M * 1.2M), 0.95M, true, false, 4.3M, "https://images.unsplash.com/photo-1473966968600-fa801b869a1a?q=80&w=2000&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D", "Women");
        AddProductIfNotExisting(43, "Black", "Formal Shoes", "Perfect for business attire", "Shoes", "Formal", "10", 89.99M, (89.99M * 1.2M), 0.8M, false, true, 4.7M, "https://images.unsplash.com/photo-1609259886986-a642e7e1dbf9?w=600&auto=format&fit=crop&q=60&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxzZWFyY2h8MTB8fGZvcm1hbCUyMHNob2VzfGVufDB8fDB8fHww", "Women");
        AddProductIfNotExisting(77, "Red", "Sneaky Shoes", "Perfect for robbing attire", "Shoes", "Formal", "16", 89.99M, (89.99M * 1.2M), 0.9M, false, true, 5.0M, "https://images.unsplash.com/photo-1529925692078-9eca152749cd?q=80&w=2670&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D", "Women");

        AddProductIfNotExisting(21, "Blue", "Denim Jeans", "Durable and classic style", "Pants", "Casual", "M", 39.99M, (39.99M * 1.2M), 0.9M, true, false, 4.6M, "https://images.unsplash.com/photo-1587142198902-27b362ec78e4?q=80&w=1974&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D", "Men");
        AddProductIfNotExisting(17, "Red", "Hoodie", "Soft and cozy with a warm hood", "Sweatshirts", "Casual", "S", 29.99M, (29.99M * 1.2M), 0.85M, true, false, 4.8M, "https://images.unsplash.com/photo-1606913419164-959745d50efa?q=80&w=1974&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D", "Women");
        AddProductIfNotExisting(25, "Navy", "Blazer", "Sharp for professional occasions", "Jackets", "Formal", "M", 99.99M, (99.99M * 1.2M), 0.75M, false, true, 4.5M, "https://images.unsplash.com/photo-1582274528667-1e8a10ded835?q=80&w=1964&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D", "Boys");
        AddProductIfNotExisting(9, "Grey", "Chinos", "A smart-casual essential", "Pants", "Casual", "L", 59.99M, (59.99M * 1.2M), 0.9M, true, true, 4.4M, "https://images.unsplash.com/photo-1499202977705-65f436dac18a?q=80&w=2071&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D", "Women");
        AddProductIfNotExisting(15, "Blue", "Polo Shirt", "Comfort with a collar", "T-Shirts", "Casual", "M", 24.99M, (24.99M * 1.2M), 0.85M, false, false, 4.2M, "https://images.unsplash.com/photo-1604006852748-903fccbc4019?q=80&w=1974&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D", "Men");
        AddProductIfNotExisting(38, "Brown", "Leather Belt", "Classic accessory for any outfit", "Accessories", "Casual", "One size", 19.99M, (19.99M * 1.2M), 0.9M, true, false, 4.7M, "https://images.unsplash.com/photo-1611937685025-8d1df67a80b6?q=80&w=2070&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D", "Boys");
        AddProductIfNotExisting(2, "Charcoal", "Suit Vest", "Sleek and tailored", "Suits", "Formal", "L", 34.99M, (34.99M * 1.2M), 0.8M, false, true, 4.5M, "https://images.unsplash.com/photo-1514222788835-3a1a1d5b32f8?q=80&w=1974&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D", "Women");
        AddProductIfNotExisting(29, "Brown", "Cardigan", "Layer up with style", "Sweaters", "Casual", "M", 49.99M, (49.99M * 1.2M), 0.8M, true, false, 4.3M, "https://images.unsplash.com/photo-1616821074314-245674337a0d?q=80&w=1974&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D", "Boys");

        AddProductIfNotExisting(51, "Beige", "Trench Coat", "Elegant and comfortable", "Coats", "Casual", "XL", 89.99M, (89.99M * 1.2M), 0.85M, true, false, 4.7M, "https://images.unsplash.com/photo-1617391258031-f8d80b22fb35?q=80&w=764&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D", "Boys");
        AddProductIfNotExisting(52, "Black", "Skinny Jeans", "Versatile and modern", "Pants", "Casual", "M", 45.99M, (45.99M * 1.2M), 0.85M, true, true, 4.6M, "https://images.unsplash.com/photo-1603217192634-61068e4d4bf9?q=80&w=735&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D", "Women");
        AddProductIfNotExisting(53, "White", "Sneakers", "Comfortable for all-day wear", "Shoes", "Casual", "42", 59.99M, (59.99M * 1.2M), 0.8M, true, true, 4.8M, "https://images.unsplash.com/photo-1580902215262-9b941bc6eab3?q=80&w=774&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D", "Women");
        AddProductIfNotExisting(54, "Olive", "Parka", "Stay warm and stylish", "Coats", "Casual", "L", 119.99M, (119.99M * 1.2M), 0.7M, false, true, 4.5M, "https://images.unsplash.com/photo-1561778233-89714b6f2033?q=80&w=735&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D", "Women");
        AddProductIfNotExisting(55, "White", "Sweatshirt", "Classic comfort fit", "Sweatshirts", "Casual", "S", 35.99M, (35.99M * 1.2M), 0.9M, true, false, 4.3M, "https://images.unsplash.com/photo-1635892438808-e6dcf5f43b17?q=80&w=687&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D", "Women");
        AddProductIfNotExisting(56, "Maroon", "Pullover", "Soft knit for chilly days", "Sweaters", "Casual", "M", 29.99M, (29.99M * 1.2M), 0.85M, false, false, 4.4M, "https://images.unsplash.com/photo-1588187284031-cb3fa95ebb27?q=80&w=735&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D", "Women");
        AddProductIfNotExisting(57, "Black", "Chukka Boots", "Dress up or down", "Shoes", "Casual", "11", 79.99M, (79.99M * 1.2M), 0.85M, true, true, 4.7M, "https://images.unsplash.com/photo-1521161235081-1f79d1d08236?q=80&w=758&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D", "Boys");
        AddProductIfNotExisting(58, "Crimson", "Beanie", "Warm and snug fit", "Accessories", "Casual", "One size", 14.99M, (14.99M * 1.2M), 0.9M, true, false, 4.5M, "https://images.unsplash.com/photo-1482840927633-621e8149f04f?q=80&w=870&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D", "Boys");
        AddProductIfNotExisting(59, "White", "Dress Shirt", "Sharp and versatile", "Shirts", "Formal", "L", 39.99M, (39.99M * 1.2M), 0.85M, false, true, 4.6M, "https://images.unsplash.com/photo-1611619668988-9dd258b42ed3?q=80&w=687&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D", "Women");
        AddProductIfNotExisting(60, "Pink", "Pencil Skirt", "Sleek office wear", "Skirts", "Formal", "M", 29.99M, (29.99M * 1.2M), 0.8M, true, false, 4.4M, "https://images.unsplash.com/photo-1646054224885-f978f5798312?q=80&w=687&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D", "Girls");

    }
    private void AddProductIfNotExisting(int quantity, string color, string productName, string? description, string? category, string? type, string? size, decimal priceExcTax, decimal priceIncTax, decimal salePricePercentage, bool isOnSale, bool isFeatured, decimal rating, string? imageUrl, string gender)
    {
        EntityEntry<Product> addedProduct = null!;
        var productCategory = _context.Categories.FirstOrDefault(x => x.CategoryName.ToLower() == category.ToLower());

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
                Gender = gender,
                CategoryId = productCategory.Id,
                ProductCategory = productCategory,
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
    private void SeedPromoCodes()
    {
        AddPromoCodeIfNotExisting("Sigma Apparel", .30M, null, false, DateTime.Now.AddDays(30), "HurryUpSale23", "https://images.unsplash.com/photo-1607082350899-7e105aa886ae?q=80&w=2070&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D");
        AddPromoCodeIfNotExisting("Shoes4U", .50M, null, false, DateTime.Now.AddDays(15), "BlackWeek2023", "https://images.unsplash.com/photo-1543163521-1bf539c55dd2?w=500&auto=format&fit=crop&q=60&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxzZWFyY2h8Mnx8YXBwYXJlbHxlbnwwfHwwfHx8MA%3D%3D");
        AddPromoCodeIfNotExisting("Union Pants co.", .50M, null, false, DateTime.Now.AddDays(45), "JustForYou12", "https://images.unsplash.com/photo-1607082349566-187342175e2f?q=80&w=2070&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D");
        AddPromoCodeIfNotExisting("Light is Right Inc.", .20M, null, true, DateTime.Now.AddDays(25), "SummerSale20", "https://images.unsplash.com/photo-1607083206968-13611e3d76db?q=80&w=2115&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D");
        AddPromoCodeIfNotExisting("Scandi Jackets Co.", .20M, null, false, DateTime.Now.AddDays(20), "WinterJackets20Off", "https://images.unsplash.com/photo-1577538928305-3807c3993047?q=80&w=2070&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D");
        AddPromoCodeIfNotExisting("Future is Here Corp.", .10M, null, true, DateTime.Now.AddDays(10), "AllElectronics10Percent", "https://images.unsplash.com/photo-1577387196112-579d95312c6d?q=80&w=2070&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D");
        AddPromoCodeIfNotExisting("Around the corner co.", .50M, null, false, DateTime.Now.AddDays(14), "StoreClearance", "https://images.unsplash.com/photo-1607083206869-4c7672e72a8a?q=80&w=2070&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D");
        AddPromoCodeIfNotExisting("Brother 4 ever", .15M, null, true, DateTime.Now.AddDays(3), "FallIsHereSale15Percent", "https://images.unsplash.com/photo-1561069934-eee225952461?q=80&w=2070&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D");
        AddPromoCodeIfNotExisting("Same are all by SAME Co. Ltd.", .15M, null, true, DateTime.Now.AddDays(7), "SameLikeOthers", "https://images.unsplash.com/photo-1607083207685-aaf05f2c908c?q=80&w=2070&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D");
        AddPromoCodeIfNotExisting("Brother 4 ever", .15M, null, true, DateTime.Now.AddDays(1), "LimitedOffer23", "https://images.unsplash.com/photo-1531303435785-3853ba035cda?q=80&w=2070&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D");
    }
    private void AddPromoCodeIfNotExisting(string name, decimal percentage, decimal? amount, bool isUsed, DateTime validity, string codeText, string imgUrl)
    {
        var promoCodeExists = _context.PromoCodes.Any(x => x.PromoCodeText == codeText);
        if(!promoCodeExists)
        {
            var promoCode = new PromoCode
            {
                PromoCodeName = name,
                PromoCodePercentage = percentage,
                PromoCodeAmount = amount,
                PromoCodeIsUsed = isUsed,
                PromoCodeValidity = validity,
                PromoCodeText = codeText,
                PromoCodeImgUrl = imgUrl
            };

            _context.PromoCodes.Add(promoCode);
            _context.SaveChanges();

            var users = _context.Users.ToList();

            var random = new Random();
            var user = users[random.Next(users.Count)];

            var userPromoCode = new UserPromoCode();
            if (users is not null)
            {
                userPromoCode.Id = user.Id;
                userPromoCode.PromoCode = promoCode;

                _context.UserPromoCodes.Add(userPromoCode);
                _context.SaveChanges();
            }
        }
    }
}
