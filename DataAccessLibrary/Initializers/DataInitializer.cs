
using DataAccessLibrary.Contexts;
using DataAccessLibrary.Entities;
using DataAccessLibrary.Entities.ProductEntities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System.Diagnostics;
using DataAccessLibrary.Entities.UserEntities;
using System.Data;


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

    }
    private void SeedReviews() { }
    private void SeedCards() { }

    private void AddAddressIfNotExists()
    {

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

    private void AddProductIfNotExisting(int quantity, string colorName, string productName, string? description, string? category, string? type, string? size, decimal price, decimal salePrice, bool isOnSale, bool isFeatured, decimal rating, string? imageUrl)
    {
        EntityEntry<Product> addedProduct = null;
        EntityEntry<Color> addedColor = null;

        if (!_context.Products.Any(p => p.ProductName == productName))
        {
            var product = new Product
            {
                ProductName = productName,
                Description = description,
                Category = category,
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
        }

        if (!_context.Colors.Any(p => p.ColorName == colorName))
        {
            var color = new Color
            {
                ColorName = colorName
            };
            addedColor = _context.Colors.Add(color);
        }

        try
        {
            _context.SaveChanges();

            if (addedProduct != null)
            {
                addedProduct.Entity.GenerateProductNumber();
                _context.Update(addedProduct.Entity);
            }

            if (addedProduct != null && addedColor != null)
            {
                var productColor = new ProductColor
                {
                    ColorId = addedColor.Entity.ColorId,
                    ProductId = addedProduct.Entity.ProductId,
                };
                _context.ProductColors.Add(productColor);
            }
            _context.SaveChanges();
        }
        catch (Exception ex) { Debug.WriteLine(ex.Message); Debug.WriteLine(ex.StackTrace); }
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
