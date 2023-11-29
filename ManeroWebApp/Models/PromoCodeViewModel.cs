using ServiceLibrary.Models;

namespace ManeroWebApp.Models
{
    public class PromoCodeViewModel
    {
        public string PromoCodeName { get; set; } = null!;
        public decimal PromoCodePercentage { get; set; }
        public decimal? PromoCodeAmount { get; set; }
        public bool PromoCodeIsUsed { get; set; }
        public DateTime PromoCodeValidity { get; set; }
        public string PromoCodeText { get; set; } = null!;
        public string? PromoCodeImgUrl { get; set;}


        public static implicit operator PromoCodeViewModel(PromoCode promoCode)
        {
            return new PromoCodeViewModel
            {
                PromoCodeName = promoCode.PromoCodeName,
                PromoCodePercentage = promoCode.PromoCodePercentage,
                PromoCodeAmount = promoCode.PromoCodeAmount,
                PromoCodeIsUsed = promoCode.PromoCodeIsUsed,
                PromoCodeValidity = promoCode.PromoCodeValidity,
                PromoCodeText = promoCode.PromoCodeText
            };
        }
    }
}
