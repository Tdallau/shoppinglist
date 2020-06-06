using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using my_shoppinglist_api.Hubs;
using my_shoppinglist_api.Interfaces;
using my_shoppinglist_api.Interfaces.hubs;
using my_shoppinglist_api.Models;
using my_shoppinglist_api.Models.Database;

namespace my_shoppinglist_api.Controllers
{
  public class ShopController : BaseController
  {
    private readonly IHubContext<ShopHub> _shopHub;
    private readonly IShopService _shopService;
    public ShopController(IAuthService authService, IShopService shopService, IHubContext<ShopHub> shopHub, IConfiguration config) : base(authService, config)
    {
      _shopHub = shopHub;
      _shopService = shopService;
    }

    /// <summary>
    /// Get all the shops from the default shoppingroup of the user
    /// </summary>
    /// <returns>All the shops of the default shoppinggroup</returns>
    /// <response code="200">Returns all the shops of the default shoppinggroup</response>
    /// <response code="400">No user found by this JWT token!</response>
    /// <response code="401">User is not loggedin</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<BaseResponse<ListResponse<ShopResponse>>>> GetShops()
    {
      var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault();
      var user = await _authService.CheckToken(token);
      if (user == null) return BadRequest(new BaseResponse<ListResponse<ShopResponse>>() {
        Succes = false,
        Error = "No user found by this JWT token!"
      });


      var shops = await _shopService.GetShops(user.Id);
      var list = shops == null ? new List<ShopResponse>() : shops;

      return Ok(
          new BaseResponse<ListResponse<ShopResponse>>()
          {
            Succes = true,
            Data = new ListResponse<ShopResponse>() {
              List = list,
              Count = list.Count()
            }
          }
      );
    }

    /// <summary>
    /// Create a new shop in a shoppingroup
    /// </summary>
    /// <returns>returns response object with information about proces</returns>
    /// <response code="201">shop is created</response>
    /// <response code="400">The new shop does not have a name</response>
    /// <response code="401">User is not loggedin</response>
    /// <param name="shop"></param>     
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> CreateNewShop([FromBody] Shop shop)
    {
      var newShop = await _shopService.CreateShop(shop);
      if (newShop == null) return BadRequest(new BaseResponse<string>()
      {
        Succes = false,
        Error = "Shopname is required"
      });

      await _shopHub.Clients.Group("Test-5").SendAsync(nameof(IShopHub.NewShopCreated), newShop);
      return Created("https://shoppinglist.dallau.com", newShop);
    }


    /// <summary>
    /// Update a shop
    /// </summary>
    /// <returns>returns response object with information about proces</returns>
    /// <response code="200">returns if shop is succesfuly updated</response>
    /// <response code="400">The updated shop does not have a name or shop does not exist</response>
    /// <response code="401">User is not loggedin</response>
    /// <param name="id"></param>     
    /// <param name="shop"></param>   
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<string>> UpdateShop([FromRoute] int id, [FromBody] Shop shop)
    {
      var updatedShop = await _shopService.UpdateShop(id, shop);
      if (updatedShop == null) return BadRequest(new BaseResponse<string>()
      {
        Succes = false,
        Error = "Shop not found"
      });

      await _shopHub.Clients.Group("Test-5").SendAsync(nameof(IShopHub.UpdateShop), id, updatedShop);
      return Ok(new BaseResponse<string>()
      {
        Data = "Shop is updated",
        Succes = true,
      });
    }

    /// <summary>
    /// Delete a shop
    /// </summary>
    /// <returns>returns response object with information about proces</returns>
    /// <response code="200">returns if shop is succesfuly deleted</response>
    /// <response code="400">shop does not exist</response>
    /// <response code="401">User is not loggedin</response>
    /// <param name="id"></param>     
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<BaseResponse<string>>> deleteShop([FromRoute] int id)
    {
      var isDeleted = await _shopService.DeleteShop(id);
      if (!isDeleted) return BadRequest(new BaseResponse<string>()
      {
        Succes = false,
        Error = "Shop not found"
      });

      await _shopHub.Clients.Group("Test-5").SendAsync(nameof(IShopHub.DeleteShop), id);
      return Ok(new BaseResponse<string>()
      {
        Data = "Shop is deleted",
        Succes = true,
      });
    }
  }
}