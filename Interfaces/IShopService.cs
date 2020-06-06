using System.Collections.Generic;
using System.Threading.Tasks;
using my_shoppinglist_api.Models;
using my_shoppinglist_api.Models.Database;

namespace my_shoppinglist_api.Interfaces
{
    public interface IShopService
    {
         Task<IEnumerable<ShopResponse>> GetShops(int userId);
         Task<ShopResponse> GetShopByShopId(int shopId);
         Task<Shop> CreateShop(Shop shop);
         Task<Shop> UpdateShop(int id, Shop shop);
         Task<bool> DeleteShop(int id);
    }
}