using System.Text.Json.Serialization;

namespace my_shoppinglist_api.Models.Database
{
  public class ShoppingGroupUser
  {
    public int ShoppingGroupId { get; set; }
    public int UserId { get; set; }

    public bool Default { get; set; }

    [JsonIgnore]
    public virtual ShoppingGroup ShoppingGroup { get; set; }
    [JsonIgnore]
    public virtual User User { get; set; }
  }
}