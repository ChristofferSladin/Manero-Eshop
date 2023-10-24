
using DataAccessLibrary.Entities.ProductEntities;
using DataAccessLibrary.Entities.UserEntities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;

namespace DataAccessLibrary.Contexts;

public class ManeroDbContext : IdentityDbContext
{
    public ManeroDbContext(DbContextOptions<ManeroDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<Order>().Property(e => e.OrderStatus).HasConversion<string>();

        builder.Entity<Product>()
        .HasIndex(u => u.ProductNumber)
        .IsUnique();

        builder.Entity<Order>()
        .HasIndex(u => u.OrderNumber)
        .IsUnique();
    }

    public DbSet<Product> Products { get; set; }
    public DbSet<Color> Colors { get; set; }
    public DbSet<ProductColor> ProductColors { get; set; }
    public DbSet<Review> Reviews { get; set; }
    public DbSet<Adress> Adresses { get; set; }
    public DbSet<Card> Cards { get; set; }
    public DbSet<FavoriteProduct> FavoriteProducts { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<ShoppingCart> ShoppingCarts { get; set; }
    public DbSet<ApplicationUser> ApplicationUsers { get; set; }
}
