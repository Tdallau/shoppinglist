using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using my_shoppinglist_api.Helpers.Context;
using my_shoppinglist_api.Interfaces;
using my_shoppinglist_api.Models;
using my_shoppinglist_api.Models.Database;
using my_shoppinglist_api.Models.grocceryItem;

namespace my_shoppinglist_api.Services
{
  public class GroceryItemService : IGroceryItemService
  {
    private readonly MainContext _context;
    public GroceryItemService(MainContext context)
    {
      _context = context;
    }

    public async Task<GroceryList> GetGrocceryListByShopId(int shopId)
    {
      var shop = await _context.Shop.FirstOrDefaultAsync(x => x.Id == shopId);
      if (shop == null) return null;
      byte[] signalrRoom = System.Text.ASCIIEncoding.ASCII.GetBytes($"{shop.Name}-{shop.Id}");
      var grocceries = _context.GroceryItem.Where(x => x.ShopId == shopId).OrderBy(x => x.Purchased);
      var grocceryList = new GroceryList()
      {
        ShopId = shopId,
        ShopName = shop.Name,
        signalrRoom = Convert.ToBase64String(signalrRoom),
        Grocceries = new ListResponse<GroceryItem>() {
          List = grocceries,
          Count = grocceries.Count()
        }
      };

      return grocceryList;
    }

    public async Task<GroceryItem> AddItemToList(int shopId, GroceryItem grocery)
    {
      if (String.IsNullOrWhiteSpace(grocery.Name)) return null;
      var shop = await _context.Shop.FirstOrDefaultAsync(x => x.Id == shopId);
      if (shop == null) return null;
      grocery.Purchased = false;
      grocery.ShopId = shopId;
      grocery.AddedAt = DateTime.Now;

      await _context.AddAsync(grocery);
      await _context.SaveChangesAsync();
      return grocery;
    }

    public async Task<GroceryItem> CheckProduct(int shopid, int groceryId)
    {
      var grocery = await _context.GroceryItem.FirstOrDefaultAsync(x => x.Id == groceryId && x.ShopId == shopid);
      if(grocery == null) return null;

      grocery.Purchased = !grocery.Purchased;
      _context.Update(grocery);
      await _context.SaveChangesAsync();
      return grocery;
    }

    public async Task<string> GetSignalrRoom(int shopId)
    {
      var shop = await _context.Shop.FirstOrDefaultAsync(x => x.Id == shopId);
      if (shop == null) return null;
      byte[] signalrRoom = System.Text.ASCIIEncoding.ASCII.GetBytes($"{shop.Name}-{shop.Id}");
      return Convert.ToBase64String(signalrRoom);
    }
  }
}