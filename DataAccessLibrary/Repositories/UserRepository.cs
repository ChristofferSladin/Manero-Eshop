using DataAccessLibrary.Contexts;
using DataAccessLibrary.Entities;
using Microsoft.AspNetCore.Identity;

namespace DataAccessLibrary.Repositories;

public class UserRepository : Repository<IdentityUser>
{
    public UserRepository(ManeroDbContext context) : base(context)
    {
    }
}
