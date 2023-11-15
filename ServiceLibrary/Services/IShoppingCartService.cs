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
        Task<List<ShoppingCartProduct>> GetUserShoppingCartProductsAsync(string user);
        Task<HttpResponseMessage> AddProductToShoppingCartAsync(string user, int itemQuantity, string productNumber);
    }
}
