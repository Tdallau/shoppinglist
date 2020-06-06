using System.Threading.Tasks;
using my_shoppinglist_api.Models.Database;

namespace my_shoppinglist_api.Interfaces.hubs
{
    public interface IGroceryItemHub
    {
        Task NewGroceryAdded(GroceryItem grocery);
        Task CheckGrocery(int shopId, GroceryItem grocery);
    }
}