using DataAccessLibrary.Contexts;
using DataAccessLibrary.Entities;
using DataAccessLibrary.Entities.ProductEntities;
using DataAccessLibrary.Entities.UserEntities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DataAccessLibrary.Repositories;

public class UserRepository : Repository<IdentityUser>
{
    private readonly ManeroDbContext _context;
    public UserRepository(ManeroDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<UserProfile> GetUserByIdAsync(Expression<Func<UserProfile, bool>> expression)
    {
        return await _context.UserProfiles.FirstOrDefaultAsync(expression) ?? null!;
    }
}
