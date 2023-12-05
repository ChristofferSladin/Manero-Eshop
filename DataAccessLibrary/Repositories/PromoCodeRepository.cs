using DataAccessLibrary.Contexts;
using DataAccessLibrary.Entities.UserEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLibrary.Repositories
{
    public class PromoCodeRepository : Repository<PromoCode>
    {
        public PromoCodeRepository(ManeroDbContext context) : base(context)
        {
        }
    }
}
