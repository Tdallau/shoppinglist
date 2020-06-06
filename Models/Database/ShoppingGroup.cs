using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace my_shoppinglist_api.Models.Database
{
  public class ShoppingGroup
  {
    public int Id { get; set; }
    public int OwnerId { get; set; }
    public string Name { get; set; }

    [JsonIgnore]
    public virtual List<ShoppingGroupUser> ShoppingGroupUsers { get; set; }
    [JsonIgnore]
    public virtual List<Shop> Shops { get; set; }
    [JsonIgnore]
    public virtual User Owner { get; set; }
  }
}