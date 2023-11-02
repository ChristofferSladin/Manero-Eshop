using DataAccessLibrary.Entities.ProductEntities;
using DataAccessLibrary.Entities.UserEntities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ManeroWebApp.Models
{
    public class ProductViewModel
    {
        public int ProductId { get; set; }
        public string? ProductNumber { get; private set; }

        public string ProductName { get; set; } = null!;

        public string? Description { get; set; }

        public string? Category { get; set; }

        public string? Type { get; set; }

        public string? Size { get; set; }

        public int QuantityInStock { get; set; }

        public string? Color { get; set; }

        public decimal PriceExcTax { get; set; }

        public decimal PriceIncTax { get; set; }

        public decimal SalePrice { get; set; }

        public bool IsOnSale { get; set; }

        public bool IsFeatured { get; set; }

        public decimal Rating { get; set; }

        public string? ImageUrl { get; set; }

    }
}
