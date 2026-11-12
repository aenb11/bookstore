using Bookstore.Api.Common;
using Bookstore.Api.DTOs.Addresses;
using Bookstore.Api.Services.Storefront;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bookstore.Api.Controllers.Storefront;

[ApiController]
[Route("api/v1/addresses")]
[Authorize(Roles = $"{AppRole.Customer}")]
public class AddressController(IAddressService addressService) : ControllerBase
{
    [HttpGet("me")]
    public async Task<IActionResult> GetMyAddressesAsync(CancellationToken ct = default)
    {
        var accountId = User.GetAccountId();
        var result = await addressService.GetAllAsync(accountId, ct);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync(
        [FromBody] AddressPostDto dto,
        CancellationToken ct = default
    )
    {
        var accountId = User.GetAccountId();
        var result = await addressService.CreateAsync(accountId, dto, ct);
        return StatusCode(StatusCodes.Status201Created, result);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateAsync(
        int id,
        [FromBody] AddressPutDto dto,
        CancellationToken ct = default
    )
    {
        var accountId = User.GetAccountId();
        var result = await addressService.UpdateAsync(accountId, id, dto, ct);
        if (result is null)
            return Problem(statusCode: StatusCodes.Status404NotFound, title: $"Address not found.");

        return Ok(result);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteAsync(int id, CancellationToken ct = default)
    {
        var accountId = User.GetAccountId();
        await addressService.DeleteAsync(accountId, id, ct);
        return NoContent();
    }
}
