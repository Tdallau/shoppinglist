using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using my_shoppinglist_api.Helpers.Context;
using my_shoppinglist_api.Interfaces.hubs;

namespace my_shoppinglist_api.Hubs
{
  [Authorize]
  public class ShopHub : Hub<IShopHub>
  {
    private readonly MainContext _context;
    public ShopHub(MainContext context)
    {
      _context = context;
    }

    public async Task ConnectToShoppingGroup()
    {
      var shoppingGroupUser = await _context.ShoppingGroupUser.FirstOrDefaultAsync(x => x.UserId == Int32.Parse(Context.UserIdentifier) && x.Default);
      if (shoppingGroupUser != null)
      {
        var shoppinGroup = await _context.ShoppingGroup.FirstOrDefaultAsync(x => x.Id == shoppingGroupUser.ShoppingGroupId);
        if (shoppinGroup != null)
        {
          var groupName = $"{shoppinGroup.Name}-{shoppinGroup.Id}";

          await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        }
      }
    }

    public async Task DisconectFromShoppingGroup()
    {
      await Groups.RemoveFromGroupAsync(Context.ConnectionId, "Test-5");
    }
  }
}