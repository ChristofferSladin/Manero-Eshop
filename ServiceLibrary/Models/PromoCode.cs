using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLibrary.Models
{
    public class PromoCode
    {
        public string PromoCodeName { get; set; } = null!;
        public decimal PromoCodePercentage { get; set; }
        public decimal? PromoCodeAmount { get; set; }
        public bool PromoCodeIsUsed { get; set; }
        public DateTime PromoCodeValidity { get; set; }
        public string PromoCodeText { get; set; } = null!;
        public string? PromoCodeImgUrl { get; set; }

    }
}
