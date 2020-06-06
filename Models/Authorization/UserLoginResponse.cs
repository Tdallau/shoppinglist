using System;
using my_shoppinglist_api.Helpers.enums;

namespace my_shoppinglist_api.Models.Authorization
{
  public class UserLoginResponse
  {
    public int Id { get; set; }
    public string Email { get; set; }
    public Role Role { get; set; }
    public int? DefaultShoppingGroupId { get; set; }
    public JWTToken Token { get; set; }
  }

  public class JWTToken
  {
    public string JwtToken { get; set; }
    public string RefreshToken { get; set; }
    public long Nbf { get; set; }
    public long Exp { get; set; }
  }


}