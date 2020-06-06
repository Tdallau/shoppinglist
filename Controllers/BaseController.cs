using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using my_shoppinglist_api.Interfaces;

namespace my_shoppinglist_api.Controllers
{
  [Produces("application/json")]
  [EnableCors("SiteCorsPolicy")]
  [Authorize]
  [Route("api/[controller]")]
  public class BaseController : ControllerBase
  {
    protected readonly IAuthService _authService;
    protected readonly IConfiguration _config;
    public BaseController(IAuthService authService, IConfiguration config)
    {
      _authService = authService;
      _config = config;
    }m
  }
}