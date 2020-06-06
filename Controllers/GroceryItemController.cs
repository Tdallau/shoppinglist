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
using my_shoppinglist_api.Models.grocceryItem;

namespace my_shoppinglist_api.Controllers
{
  public class GroceryItemController : BaseController
  {
    private readonly IGroceryItemService _grocceryItemService;
    private readonly IShopService _shopService;
    private readonly IHubContext<GroceryItemHub> _groceryItemHub;
    private readonly IHubContext<ShopHub> _shopHub;
    public GroceryItemController(IAuthService authService, IGroceryItemService grocceryItemService, IShopService shopService, IHubContext<GroceryItemHub> groceryItemHub, IHubContext<ShopHub> shopHub, IConfiguration config) : base(authService, config)
    {
      _grocceryItemService = grocceryItemService;
      _shopService = shopService;
      _groceryItemHub = groceryItemHub;
      _shopHub = shopHub;
    }

    /// <summary>
    /// add new grocery to shop
    /// </summary>
    /// <returns>returns if grocery is aded to shop</returns>
    /// <response code="201">grocery is added to shoppinglist</response>
    /// <response code="400">shop does not exist</response>
    /// <response code="401">User is not loggedin</response>
    /// <param name="grocery"></param>  
    /// <param name="shopId"></param>  
    [HttpPost("{shopId}")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> AddGrocery([FromBody] GroceryItem grocery, [FromRoute] int shopId)
    {
      var newGroceryItem = await _grocceryItemService.AddItemToList(shopId, grocery);
      if (newGroceryItem == null) return BadRequest(new BaseResponse<string>() {
        Succes = false,
        Error = "no shop found with this shopId"
      });

      var signalrRoom = await _grocceryItemService.GetSignalrRoom(shopId);
      if (signalrRoom == null) return BadRequest(new BaseResponse<string>() {
        Succes = false,
        Error = "No signalr group found for this grocery list"
      });
      var shopResponse = await _shopService.GetShopByShopId(shopId);
      await _groceryItemHub.Clients.Group(signalrRoom).SendAsync(nameof(IGroceryItemHub.NewGroceryAdded), newGroceryItem);
      await _shopHub.Clients.Group("Test-5").SendAsync(nameof(IShopHub.UpdateShop), shopResponse);

      return Created("https://shoppinglist.dallau.com", newGroceryItem);
    }

    /// <summary>
    /// set a grocery as purched or not
    /// </summary>
    /// <returns>returns if grocery is aded to shop</returns>
    /// <response code="200">Returns true if succes</response>
    /// <response code="400">return false if shop or grocery does not exist</response>
    /// <response code="401">User is not loggedin</response>
    /// <param name="shopId"></param>  
    /// <param name="groceryId"></param>  
    [HttpPut("{shopId}/{groceryId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<BaseResponse<string>>> CheckGrocery([FromRoute] int shopId, [FromRoute] int groceryId)
    {
      var grocery = await _grocceryItemService.CheckProduct(shopId, groceryId);
      if (grocery == null) return BadRequest(new BaseResponse<string>() {
        Succes = false,
        Error = "No grocery found by this shopId and groceryId"
      });

      var signalrRoom = await _grocceryItemService.GetSignalrRoom(shopId);
      if (signalrRoom == null) return BadRequest(new BaseResponse<string>() {
        Succes = false,
        Error = "No signalr group found for this grocery list"
      });

      await _groceryItemHub.Clients.Group(signalrRoom).SendAsync(nameof(IGroceryItemHub.CheckGrocery), groceryId, grocery);
      return Ok(new BaseResponse<string>() {
        Data = $"Grocery is set to {(grocery.Purchased ? "purchased" : "not purchased")}",
        Succes = true
      });
    }

    /// <summary>
    /// get all grocerys on shoppinglist by shopid
    /// </summary>
    /// <returns>returns all grocerys on shoppinglist</returns>
    /// <response code="200">grocerys on shoppinglist</response>
    /// <response code="400">shop does not exist</response>
    /// <response code="401">User is not loggedin</response>
    /// <param name="shopId"></param>  
    [HttpGet("{shopId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<BaseResponse<GroceryList>>> GetGroceryListByShopId([FromRoute] int shopId)
    {
      var groceryList = await _grocceryItemService.GetGrocceryListByShopId(shopId);
      if (groceryList == null) return BadRequest(new BaseResponse<GroceryList>() {
        Succes = false,
        Error = "shop does not exist"
      });

      return Ok(new BaseResponse<GroceryList>()
      {
        Data = groceryList,
        Succes = true
      });
    }
  }
}