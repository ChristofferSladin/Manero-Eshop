using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLibrary.Models
{
    public class ShoppingCartProduct
    {
        public int ShoppingCartProductId { get; set; }
        public decimal TotalPriceIncTax { get; set; }
        public decimal TotalPriceExcTax { get; set; }
        public int ItemQuantity { get; set; }
        public int ProductId { get; set; }
        public string ProductNumber { get; set; }
        public int ShoppingCartId { get; set; }
    }
}
