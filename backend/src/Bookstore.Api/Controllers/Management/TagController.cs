using Bookstore.Api.Common;
using Bookstore.Api.DTOs.Tags;
using Bookstore.Api.Services.Management;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bookstore.Api.Controllers.Management;

[ApiController]
[Route("api/v1/management/tags")]
[Authorize(Roles = $"{AppRole.Admin}, {AppRole.Staff}")]
[Tags("Management - Tags")]
public class TagController(ITagManagementService tagManagementService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAllTags(
        [FromQuery] TagManagementFilterParam f,
        CancellationToken ct = default
    )
    {
        return Ok(await tagManagementService.GetTagsAsync(f, ct));
    }

    [HttpPost]
    public async Task<IActionResult> CreateTag(
        [FromBody] TagPostDto dto,
        CancellationToken ct = default
    )
    {
        var tag = await tagManagementService.CreateTagAsync(dto, ct);

        return CreatedAtAction(nameof(GetAllTags), new { id = tag.Id }, tag);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateTag(
        int id,
        [FromBody] TagPutDto dto,
        CancellationToken ct = default
    )
    {
        var tag = await tagManagementService.UpdateTagAsync(id, dto, ct);

        return Ok(tag);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteTag(int id, CancellationToken ct = default)
    {
        await tagManagementService.DeleteTagAsync(id, ct);

        return NoContent();
    }
}
