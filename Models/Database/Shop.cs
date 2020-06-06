using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace my_shoppinglist_api.Models.Database
{
  public class Shop
  {
    public int Id { get; set; }
    public int ShoppingGroupId { get; set; }
    public string Name { get; set; }

    [JsonIgnore]
    public virtual ShoppingGroup ShoppingGroup { get; set; }
    [JsonIgnore]
    public virtual List<GroceryItem> GroceryItems { get; set; }
  }
}