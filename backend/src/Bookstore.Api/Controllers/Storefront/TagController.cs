using Bookstore.Api.DTOs.Tags;
using Bookstore.Api.Services.Storefront;
using Microsoft.AspNetCore.Mvc;

namespace Bookstore.Api.Controllers.Storefront;

[ApiController]
[Route("api/v1/tags")]
public class TagController(ITagService tagService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetTags([FromQuery] TagFilterParam f, CancellationToken ct)
    {
        var tags = await tagService.GetTagsAsync(f, ct);
        return Ok(tags);
    }
}
