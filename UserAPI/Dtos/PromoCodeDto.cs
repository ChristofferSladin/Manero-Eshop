using DataAccessLibrary.Entities.UserEntities;
using System.ComponentModel.DataAnnotations;

namespace UserAPI.Dtos
{
    public class PromoCodeDto
    {
        public string PromoCodeName { get; set; } = null!;
        public decimal PromoCodePercentage { get; set; }
        public decimal? PromoCodeAmount { get; set; }
        public bool PromoCodeIsUsed { get; set; }
        public DateTime PromoCodeValidity { get; set; }
        public string PromoCodeText { get; set; } = null!;
        public string? PromoCodeImgUrl { get; set; }


        public static implicit operator PromoCodeDto(PromoCode promoCode)
        {
            return new PromoCodeDto
            {
                PromoCodeName = promoCode.PromoCodeName,
                PromoCodePercentage = promoCode.PromoCodePercentage,
                PromoCodeIsUsed = promoCode.PromoCodeIsUsed,
                PromoCodeAmount = promoCode.PromoCodeAmount,
                PromoCodeValidity = promoCode.PromoCodeValidity,
                PromoCodeText = promoCode.PromoCodeText,
                PromoCodeImgUrl = promoCode.PromoCodeImgUrl
            };
        }
    }
}
