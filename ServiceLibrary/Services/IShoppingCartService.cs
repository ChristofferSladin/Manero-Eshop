using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceLibrary.Models;

namespace ServiceLibrary.Services
{
    public interface IShoppingCartService
    {
        Task<List<ShoppingCartProduct>> GetUserShoppingCartProductsAsync();
        Task<HttpResponseMessage> AddProductToShoppingCartAsync(int itemQuantity, string productNumber);
    }
}
