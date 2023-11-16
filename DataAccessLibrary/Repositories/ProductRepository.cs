﻿using System.Linq.Expressions;
using DataAccessLibrary.Contexts;
using DataAccessLibrary.Entities.ProductEntities;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLibrary.Repositories;
public class ProductRepository
{
    private readonly ManeroDbContext _context;
    public ProductRepository(ManeroDbContext context)
    {
        _context = context;
    }
    public async Task<Product> GetProductAsync(Expression<Func<Product, bool>> expression)
    {
        return await _context.Products.FirstOrDefaultAsync(expression) ?? null!;
    }
    public async Task<Product> GetProductWithReviewsAsync(Expression<Func<Product, bool>> expression)
    {
        return await _context.Products.Include(r => r.Reviews).FirstOrDefaultAsync(expression) ?? null!;
    }
    public async Task<List<Product>> GetProductsAsync()
    {
        return await _context.Products.ToListAsync();
    }
    public async Task<List<Product>> GetProductsWithReviewsAsync()
    {
        return await _context.Products.Include(r => r.Reviews).ToListAsync();
    }
    public async Task<List<Product>> GetOnSaleProductsWithReviewsAsync()
    {
        return await _context.Products.Where(p => p.IsOnSale).Include(r => r.Reviews).ToListAsync();
    }
    public async Task<List<Product>> GetFeaturedProductsWithReviewsAsync()
    {
        return await _context.Products.Where(p => p.IsFeatured).Include(r => r.Reviews).ToListAsync();
    }
    public async Task<List<Product>> GetFilteredProductsAsync(int skip, int take, Expression<Func<Product, bool>> filterByName = null!, Expression<Func<Product, bool>> filterByCategory = null!, Expression<Func<Product, dynamic>> orderByField = null!, string orderDirection = null!)
    {
        var query = _context.Products.AsQueryable();

        if (filterByCategory != null!)
        {
            query = query.Where(filterByCategory);
        }

        if (filterByName != null!)
        {
            query = query.Where(filterByName);
        }

        if (orderByField != null!)
        {
            query = orderDirection switch
            {
                "asc" => query.OrderBy(orderByField),
                "desc" => query.OrderByDescending(orderByField),
                _ => query
            };
        }

        if (take == 0)
        {
            return await query.ToListAsync();
        }

        return await query.Skip(skip).Take(take).ToListAsync();
    }
    public async Task<List<Product>> GetFilteredProductsWithReviewsAsync(int skip, int take, Expression<Func<Product, bool>> filterByName = null!, Expression<Func<Product, bool>> filterByCategory = null!, Expression<Func<Product, dynamic>> orderByField = null!, string orderDirection = null!)
    {
        var query = _context.Products.Include(r => r.Reviews).AsQueryable();

        if (filterByCategory != null!)
        {
            query = query.Where(filterByCategory);
        }

        if (filterByName != null!)
        {
            query = query.Where(filterByName);
        }

        if (orderByField != null!)
        {
            query = orderDirection switch
            {
                "asc" => query.OrderBy(orderByField),
                "desc" => query.OrderByDescending(orderByField),
                _ => query
            };
        }

        if (take == 0)
        {
            return await query.ToListAsync();
        }

        return await query.Skip(skip).Take(take).ToListAsync();
    }
    public async Task<List<Product>> GetFilteredProductsAsync(int skip, int take, Expression<Func<Product, bool>> filterByName = null!, Expression<Func<Product, bool>> filterByCategory = null!, Expression<Func<Product, dynamic>> orderByField = null!, string orderDirection = null!, Expression<Func<Product, bool>> gender = null!)
    {
        var query = _context.Products.AsQueryable();

        if (filterByCategory != null!)
        {
            query = query.Where(filterByCategory);
        }

        if (filterByName != null!)
        {
            query = query.Where(filterByName);
        }

        if (gender != null!)
        {
            query = query.Where(gender);
        }

        if (orderByField != null!)
        {
            query = orderDirection switch
            {
                "asc" => query.OrderBy(orderByField),
                "desc" => query.OrderByDescending(orderByField),
                _ => query
            };
        }

        if (take == 0)
        {
            return await query.ToListAsync();
        }

        return await query.Skip(skip).Take(take).ToListAsync();
    }
    public async Task<IEnumerable<string>> GetProductCategories(string categoryType)
    {
        var categoryList = new List<string>();
        List<string?> query = new();

        if(categoryType.ToLower() == "gender")
            query = await _context.Products.Select(x => x.Gender).ToListAsync();
        if(categoryType.ToLower() == "category")
            query = await _context.Products.Select(x => x.Category).ToListAsync();

        if(query is not null)
        {
            foreach (var category in query)
                if (!categoryList.Any(x => x == category))
                    categoryList.Add(category!);
            
            return categoryList;
        }
        return null!;
    }
    public async Task<IEnumerable<string>> GetProductSubCategories(string genderCategory)
    {
        var categoryList = new List<string>();
        List<string?> query = new();

        if(!string.IsNullOrEmpty(genderCategory))
            query = await _context.Products.Where(x => x.Gender == genderCategory).Select(x => x.Category).ToListAsync();

        if(query is not null)
        {
            foreach (var category in query)
                if (!categoryList.Any(x => x == category))
                    categoryList.Add(category!);
            
            return categoryList;
        }
        return null!;
    }
}
