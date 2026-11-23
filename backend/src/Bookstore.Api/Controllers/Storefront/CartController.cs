using Bookstore.Api.Common;
using Bookstore.Api.DTOs.Carts;
using Bookstore.Api.Services.Storefront;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bookstore.Api.Controllers.Storefront;

[ApiController]
[Route("api/v1/cart")]
[Authorize(Roles = $"{AppRole.Customer}")]
public class CartController(ICartService cartService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetCartItemsAsync(CancellationToken ct = default)
    {
        var accountId = User.GetAccountId();
        var result = await cartService.GetCartItemsAsync(accountId, ct);
        return Ok(result);
    }

    [HttpPost("items")]
    public async Task<IActionResult> AddItem([FromBody] CartPostDto dto, CancellationToken ct)
    {
        var accountId = User.GetAccountId();
        await cartService.AddCartItemAsync(accountId, dto, ct);
        return NoContent();
    }

    [HttpPut("items/{bookId:int}")]
    public async Task<IActionResult> UpdateItemQuantity(
        [FromRoute] int bookId,
        [FromBody] CartPutDto dto,
        CancellationToken ct
    )
    {
        var accountId = User.GetAccountId();
        await cartService.UpdateCartItemAsync(accountId, bookId, dto, ct);
        return NoContent();
    }

    [HttpDelete("items/{bookId:int}")]
    public async Task<IActionResult> RemoveItem([FromRoute] int bookId, CancellationToken ct)
    {
        var accountId = User.GetAccountId();
        await cartService.RemoveCartItemAsync(accountId, bookId, ct);
        return NoContent();
    }

    [HttpDelete]
    public async Task<IActionResult> ClearCart(CancellationToken ct)
    {
        var accountId = User.GetAccountId();
        await cartService.ClearCartAsync(accountId, ct);
        return NoContent();
    }
}
