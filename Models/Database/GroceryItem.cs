using System;
using System.Text.Json.Serialization;

namespace my_shoppinglist_api.Models.Database
{
  public class GroceryItem
  {
    public int Id { get; set; }
    public int ShopId { get; set; }
    public string Name { get; set; }
    public string Amount { get; set; }
    public DateTime AddedAt { get; set; }
    public bool Purchased { get; set; }

    [JsonIgnore]
    public virtual Shop Shop { get; set; }
  }
}