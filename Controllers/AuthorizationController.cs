using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using my_shoppinglist_api.Interfaces;
using my_shoppinglist_api.Models;
using my_shoppinglist_api.Models.Authorization;
using my_shoppinglist_api.Models.Database;

namespace my_shoppinglist_api.Controllers
{
  [AllowAnonymous]
  [Route("[controller]/[action]")]
  public class AuthorizationController : BaseController
  {
    public AuthorizationController(IAuthService authService, IConfiguration config) : base(authService, config)
    {
    }

    /// <summary>
    /// Login a user
    /// </summary>
    /// <returns>Returns the user with a jwt token</returns>
    /// <response code="200">user with jwt token</response>
    /// <response code="403">the user account is not found</response>
    /// <param name="credentials"></param>  
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<BaseResponse<UserLoginResponse>>> Login([FromBody] Credentials credentials)
    {
      var user = await _authService.Login(credentials);
      if (user == null)
      {
        return Forbid();
      }
      return Ok(new BaseResponse<UserLoginResponse>()
      {
        Data = user,
        Succes = true
      });
    }

    /// <summary>
    /// register a user
    /// </summary>
    /// <returns>Returns true or false user is registerd</returns>
    /// <response code="200">User is registered</response>
    /// <response code="403">User is not registered username or password is null</response>
    /// <param name="user"></param>  
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<BaseResponse<bool>>> Register([FromBody] User user)
    {
      var succes = await _authService.Register(user);
      if (!succes) return Unauthorized(new BaseResponse<bool>()
      {
        Succes = false,
        Error = "Username and password are required"
      });

      return Ok(new BaseResponse<bool>()
      {
        Data = succes,
        Succes = true,
      });
    }

    /// <summary>
    /// refresh the jwt token of the user
    /// </summary>
    /// <returns>Returns new jwt token and refresh token</returns>
    /// <response code="200">Returns new jwt token and refresh token</response>
    /// <response code="403">the refreshtoken has expired</response>
    /// <param name="tokens"></param>  
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<BaseResponse<JWTToken>>> Refresh([FromBody] RefreshRequest tokens)
    {
      var newTokens = await _authService.Refresh(tokens);
      if (newTokens == null) return Forbid();
      return Ok(new BaseResponse<JWTToken>()
      {
        Data = newTokens,
        Succes = true
      });
    }
  }
}