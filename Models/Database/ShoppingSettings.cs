using System.Text.Json.Serialization;
using my_shoppinglist_api.Helpers.enums;

namespace my_shoppinglist_api.Models.Database
{
  public class ShoppingSettings
  {
    public int Id { get; set; }
    public int ShoppingGroupId { get; set; }
    public int ShopId { get; set; }
    public int UserId { get; set; }
    public SortingMethod SortingMethod { get; set; }

    [JsonIgnore]
    public virtual ShoppingGroup ShoppingGroup { get; set; }
    [JsonIgnore]
    public virtual Shop Shop { get; set; }
    [JsonIgnore]
    public virtual User User { get; set; }

  }
}