using ServiceLibrary.Models;

namespace ManeroWebApp.Models
{
    public class CarouselViewModel
    {
        public string? IdSuffix { get; set; }
        public string? Title { get; set; }
        public List<Product> Products { get; set; } = null!;
        public List<Product> ProductsOnSale { get; set; } = null!;
        public List<Product> FeaturedProducts { get; set; } = null!;
    }
}
