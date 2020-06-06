using System.Collections.Generic;
using System.Text.Json.Serialization;
using my_shoppinglist_api.Helpers.enums;

namespace my_shoppinglist_api.Models.Database
{
  public class User
  {
    public int Id { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public Role Role { get; set; }

    [JsonIgnore]
    public virtual List<ShoppingGroupUser> ShoppingGroupUsers { get; set; }
    [JsonIgnore]
    public virtual List<ShoppingGroup> ShoppingGroups { get; set; }
    [JsonIgnore]
    public virtual List<UserToken> UserTokens { get; set; }
  }
}