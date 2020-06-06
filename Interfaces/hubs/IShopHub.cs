using System.Threading.Tasks;
using my_shoppinglist_api.Models.Database;

namespace my_shoppinglist_api.Interfaces.hubs
{
    public interface IShopHub
    {
         Task NewShopCreated(Shop shop);
         Task DeleteShop(int id);
         Task UpdateShop(int id, Shop shop);
    }
}