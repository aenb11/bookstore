using Bookstore.Api.Common;
using Bookstore.Api.DTOs.Accounts;
using Bookstore.Api.Services.Management;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bookstore.Api.Controllers.Management;

[ApiController]
[Route("api/v1/management/accounts")]
[Authorize(Roles = AppRole.Admin)]
[Tags("Management - Accounts")]
public class AccountController(IAccountManagementService accountManagementService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAllAsync(
        [FromQuery] AccountManagementFilterParam f,
        CancellationToken ct = default
    )
    {
        var result = await accountManagementService.GetAllAsync(f, ct);
        return Ok(result);
    }

    [HttpGet("{id:int}", Name = "GetManagementAccountById")]
    public async Task<IActionResult> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var result = await accountManagementService.GetByIdAsync(id, ct);
        if (result is null)
            return Problem(statusCode: StatusCodes.Status404NotFound, title: "Account not found");

        return Ok(result);
    }

    [HttpPatch("{id:int}")]
    public async Task<IActionResult> UpdateAsync(
        int id,
        [FromBody] AccountPatchDto dto,
        CancellationToken ct = default
    )
    {
        var result = await accountManagementService.UpdateAsync(id, dto, ct);
        return Ok(result);
    }
}
