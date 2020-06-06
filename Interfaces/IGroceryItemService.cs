using System.Threading.Tasks;
using my_shoppinglist_api.Models.Database;
using my_shoppinglist_api.Models.grocceryItem;

namespace my_shoppinglist_api.Interfaces
{
    public interface IGroceryItemService
    {
         public Task<GroceryList> GetGrocceryListByShopId(int shopId); 
         public Task<GroceryItem> AddItemToList(int shopId, GroceryItem grocery);
         public Task<GroceryItem> CheckProduct(int shopid, int groceryId);
         public Task<string> GetSignalrRoom(int shopId);
    }
}