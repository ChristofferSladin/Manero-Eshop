
using DataAccessLibrary.Contexts;
using DataAccessLibrary.Entities;
using DataAccessLibrary.Entities.ProductEntities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System.Diagnostics;


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
        AddProductIfNotExisting("Blue", "Denim Jacket", "Perfect for casual outings", "Jackets", "Casual", "M", 69.99M, 59.99M, true, false, 4.5M, "denim_jacket_url");
        AddProductIfNotExisting("Black", "Leather Jacket", "Stylish and warm", "Jackets", "Formal", "L", 129.99M, 99.99M, true, true, 5.0M, "leather_jacket_url");
        AddProductIfNotExisting("White", "T-Shirt", "Comfortable cotton t-shirt", "T-Shirts", "Casual", "M", 19.99M, 14.99M, false, false, 4.0M, "tshirt_url");
        AddProductIfNotExisting("Green", "Cargo Pants", "Roomy and comfortable", "Pants", "Casual", "L", 49.99M, 39.99M, true, false, 4.3M, "cargo_pants_url");
        AddProductIfNotExisting("Black", "Formal Shoes", "Perfect for business attire", "Shoes", "Formal", "10", 89.99M, 79.99M, false, true, 4.7M, "formal_shoes_url");
        AddProductIfNotExisting("Red", "Sneaky Shoes", "Perfect for robbing attire", "Shoes", "Formal", "16", 89.99M, 79.99M, false, true, 5.0M, "sneaky_robbing_shoes_url");
    }

    private void AddProductIfNotExisting(string colorName, string productName, string? description, string? category, string? type, string? size, decimal price, decimal salePrice, bool isOnSale, bool isFeatured, decimal rating, string? imageUrl)
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
