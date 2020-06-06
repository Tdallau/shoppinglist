using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using my_shoppinglist_api.Interfaces.hubs;

namespace my_shoppinglist_api.Hubs
{
  public class GroceryItemHub : Hub<IGroceryItemHub>
  {
    public async Task ConnectToShoppingList(string groupName)
    {
      if (groupName != null)
      {
        await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
      }
    }

    public async Task DisconectFromShoppingList(string groupName)
    {
      if (groupName != null)
      {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
      }
    }
  }
}