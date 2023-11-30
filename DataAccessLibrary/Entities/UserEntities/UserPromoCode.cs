using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLibrary.Entities.UserEntities
{
    public class UserPromoCode
    {
        [Key]
        public int UserPromoCodeId { get; set; }

        public int PromoCodeId { get; set; }
        public PromoCode PromoCode { get; set; } = null!;

        public string Id { get; set; } = null!;
        public ApplicationUser User { get; set; } = null!;
    }
}
