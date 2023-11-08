using ServiceLibrary.Models;

namespace ManeroWebApp.Models
{
    public class CarouselViewModel
    {
        public string? IdSuffix { get; set; }
        public string? Title { get; set; }
        public string? EndPoint { get; set; }
        public List<ProductViewModel> Products { get; set; } = null!;
        public List<ProductViewModel> ProductsOnSale { get; set; } = null!;
        public List<ProductViewModel> FeaturedProducts { get; set; } = null!;
    }
}
