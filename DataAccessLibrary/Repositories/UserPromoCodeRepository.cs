using DataAccessLibrary.Contexts;
using DataAccessLibrary.Entities.UserEntities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DataAccessLibrary.Repositories
{
    public class UserPromoCodeRepository : Repository<UserPromoCode>
    {
        private readonly ManeroDbContext _context;

        public UserPromoCodeRepository(ManeroDbContext context) : base(context)
        {
            _context = context;
        }

        public virtual async Task<List<PromoCode>> GetAllByUserAsync(string userId)
        {
            try
            {
                List<PromoCode> promoCodesByUser = new();
                var query = await _context.UserPromoCodes.Include(x => x.User).Include(x => x.PromoCode).Where(x => x.Id == userId).ToListAsync();

                foreach(var item in query)
                    promoCodesByUser.Add(item.PromoCode);

                return promoCodesByUser;
            }
            catch (Exception ex) { Debug.WriteLine(ex.Message); }

            return null!;
        }
    }
}
