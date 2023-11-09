using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLibrary.Models
{
    public class ShoppingCart
    {
        public int ShoppingCartId { get; set; }
        public string Id { get; set; } = null!;
    }
}
