using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using my_shoppinglist_api.Interfaces;

namespace my_shoppinglist_api.Controllers
{
  public class IsLiveController : BaseController
  {
    public IsLiveController(IAuthService authService, IConfiguration config) : base(authService, config)
    {
    }

    /// <summary>
    /// Check if api is live
    /// </summary>
    /// <returns>Returns object with live message and version number</returns>
    /// <response code="200">Object with live message and version number</response>
    [AllowAnonymous]
    [Route("/")]
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<object> IsLive()
    {
      return Ok(new
      {
        Message = "Api is live",
        Version = _config.GetValue<string>("Version")
      });
    }
  }
}