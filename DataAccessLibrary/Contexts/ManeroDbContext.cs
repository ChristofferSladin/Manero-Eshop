﻿using DataAccessLibrary.Entities;
using DataAccessLibrary.Entities.ProductEntities;
using DataAccessLibrary.Entities.UserEntities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

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
    }

    public DbSet<Product> Products { get; set; }
    public DbSet<Color> Colors { get; set; }
    public DbSet<ProductColor> ProductColors { get; set; }
    public DbSet<Review> Reviews { get; set; }
    public DbSet<Address> Addresses { get; set; }
    public DbSet<Card> Cards { get; set; }
    public DbSet<FavoriteProduct> FavoriteProducts { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<ShoppingCart> ShoppingCarts { get; set; }
    public DbSet<ApplicationUser> ApplicationUsers { get; set; }
}
