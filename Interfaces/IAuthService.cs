using System.Threading.Tasks;
using my_shoppinglist_api.Models.Authorization;
using my_shoppinglist_api.Models.Database;

namespace my_shoppinglist_api.Interfaces
{
    public interface IAuthService
    {
         Task<UserLoginResponse> Login(Credentials credentials);
         Task<bool> Register(User user);
         Task<JWTToken> Refresh(RefreshRequest tokens);
         Task<User> CheckToken(string token);
    }
}