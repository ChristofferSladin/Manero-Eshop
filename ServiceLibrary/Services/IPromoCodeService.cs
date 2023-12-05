using ServiceLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLibrary.Services
{
    public interface IPromoCodeService
    {
        Task<IEnumerable<PromoCode>> GetPromoCodesByUserAsync();
        Task<IEnumerable<PromoCode>> GetPromoCodesByUserAsync(string status);
        Task<string> LinkPromoCodeToUserAsync(string promoCodeText);
    }
}
