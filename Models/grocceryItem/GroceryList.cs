using System.Collections.Generic;
using System.Linq;
using my_shoppinglist_api.Models.Database;

namespace my_shoppinglist_api.Models.grocceryItem
{
  public class GroceryList
  {
    public int ShopId { get; set; }
    public string ShopName { get; set; }
    public string signalrRoom { get; set; }
    public ListResponse<GroceryItem> Grocceries { get; set; }
  }
}