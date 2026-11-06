using Bookstore.Api.Common;
using Bookstore.Api.DTOs.Tags;
using Bookstore.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bookstore.Api.Controllers;

[ApiController]
[Route("api/v1/tags")]
public class TagController(ITagService tagService) : ControllerBase
{
    [HttpGet]
    [Authorize(Roles = $"{AppRole.Admin}, {AppRole.Staff}")]
    public async Task<IActionResult> GetAllTags(
        [FromQuery] TagFilterParam filterParam,
        CancellationToken ct = default
    )
    {
        return Ok(await tagService.GetTagsAsync(filterParam, ct));
    }

    [HttpPost]
    [Authorize(Roles = $"{AppRole.Admin}, {AppRole.Staff}")]
    public async Task<IActionResult> CreateTag(
        [FromBody] TagPostDto dto,
        CancellationToken ct = default
    )
    {
        var tag = await tagService.CreateTagAsync(dto, ct);

        return CreatedAtAction(nameof(GetAllTags), new { id = tag.Id }, tag);
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = $"{AppRole.Admin}, {AppRole.Staff}")]
    public async Task<IActionResult> UpdateTag(
        int id,
        [FromBody] TagPutDto dto,
        CancellationToken ct = default
    )
    {
        var tag = await tagService.UpdateTagAsync(id, dto, ct);

        return Ok(tag);
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = $"{AppRole.Admin}, {AppRole.Staff}")]
    public async Task<IActionResult> DeleteTag(int id, CancellationToken ct = default)
    {
        await tagService.DeleteTagAsync(id, ct);

        return NoContent();
    }
}
