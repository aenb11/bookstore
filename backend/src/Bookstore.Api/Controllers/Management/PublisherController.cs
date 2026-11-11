using Bookstore.Api.Common;
using Bookstore.Api.DTOs.Publishers;
using Bookstore.Api.Services.Management;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bookstore.Api.Controllers.Management;

[ApiController]
[Route("api/v1/management/publishers")]
[Authorize(Roles = $"{AppRole.Admin},{AppRole.Staff}")]
[Tags("Management - Publishers")]
public class PublisherController(IPublisherManagementService publisherManagementService)
    : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAllAsync(
        [FromQuery] PublisherManagementFilterParam f,
        CancellationToken ct = default
    )
    {
        var result = await publisherManagementService.GetAllAsync(f, ct);
        return Ok(result);
    }

    [HttpGet("{id:int}", Name = "GetManagementPublisherById")]
    public async Task<IActionResult> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var result = await publisherManagementService.GetByIdAsync(id, ct);
        if (result is null)
            return Problem(
                statusCode: StatusCodes.Status404NotFound,
                title: $"Publisher not found."
            );

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync(
        [FromBody] PublisherPostDto dto,
        CancellationToken ct = default
    )
    {
        var result = await publisherManagementService.CreateAsync(dto, ct);
        return CreatedAtRoute("GetManagementPublisherById", new { id = result.Id }, result);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateAsync(
        int id,
        [FromBody] PublisherPutDto dto,
        CancellationToken ct = default
    )
    {
        var result = await publisherManagementService.UpdateAsync(id, dto, ct);
        return Ok(result);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteAsync(int id, CancellationToken ct = default)
    {
        await publisherManagementService.DeleteAsync(id, ct);
        return NoContent();
    }
}
