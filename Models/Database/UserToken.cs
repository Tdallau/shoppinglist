using System;
using System.Text.Json.Serialization;

namespace my_shoppinglist_api.Models.Database
{
  public class UserToken
  {
    public int Id { get; set; }
    public int UserId { get; set; }
    public string RefreshToken { get; set; }
    public DateTime LastUpdated { get; set; }
    public DateTime ExpiryDate { get; set; }

    [JsonIgnore]
    public virtual User User { get; set; }
  }
}