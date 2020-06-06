using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using my_shoppinglist_api.Helpers.Context;
using my_shoppinglist_api.Interfaces;
using my_shoppinglist_api.Models;
using my_shoppinglist_api.Models.Database;

namespace my_shoppinglist_api.Services
{
  public class ShopService : IShopService
  {
    private readonly MainContext _context;
    public ShopService(MainContext context)
    {
      _context = context;
    }
    public async Task<IEnumerable<ShopResponse>> GetShops(int userId)
    {
      var ShoppingGroupUser = await _context.ShoppingGroupUser.FirstOrDefaultAsync(x => x.UserId == userId && x.Default);
      if(ShoppingGroupUser == null) return null;
      
      var shops = await _context.Shop.Where(x => x.ShoppingGroupId == ShoppingGroupUser.ShoppingGroupId).ToListAsync();
      if(shops == null) return null;

      return shops.Select(shop => new ShopResponse() {
        Id = shop.Id,
        Name = shop.Name,
        ShoppingGroupId = shop.ShoppingGroupId,
        GroceryItemsString = string.Join(", ", _context.GroceryItem.Where(x => x.ShopId == shop.Id && !x.Purchased).Select(x => x.Name))
      });
    }

    public async Task<ShopResponse> GetShopByShopId(int shopId)
    {
      var shop = await _context.Shop.FirstOrDefaultAsync(x => x.Id == shopId);
      if(shop == null) return null;
      return new ShopResponse() {
        Id = shop.Id,
        Name = shop.Name,
        ShoppingGroupId = shop.ShoppingGroupId,
        GroceryItemsString = string.Join(", ", _context.GroceryItem.Where(x => x.ShopId == shop.Id && !x.Purchased).Select(x => x.Name))
      };
    }

    public async Task<Shop> CreateShop(Shop shop)
    {
      if (String.IsNullOrWhiteSpace(shop.Name)) return null;
      shop.ShoppingGroupId = 5;
      await _context.AddAsync(shop);
      await _context.SaveChangesAsync();
      return shop;
    }

    public async Task<Shop> UpdateShop(int id, Shop shop)
    {
      if (String.IsNullOrWhiteSpace(shop.Name)) return null;
      var oldShop = await _context.Shop.FirstOrDefaultAsync(x => x.Id == id);
      if (oldShop == null) return null;

      oldShop.Name = shop.Name;
      _context.Update(oldShop);
      await _context.SaveChangesAsync();
      return oldShop;
    }

    public async Task<bool> DeleteShop(int id)
    {
      var oldShop = await _context.Shop.FirstOrDefaultAsync(x => x.Id == id);
      if (oldShop == null) return false;

      _context.Remove(oldShop);
      await _context.SaveChangesAsync();
      return true;
    }
  }
}