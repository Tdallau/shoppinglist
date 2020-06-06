namespace my_shoppinglist_api.Models.Authorization
{
  public class RefreshRequest
  {
    public string JWTToken { get; set; }
    public string RefreshToken { get; set; }
  }
}