using Bookstore.Api.DTOs.Publishers;
using Bookstore.Api.Services.Storefront;
using Microsoft.AspNetCore.Mvc;

namespace Bookstore.Api.Controllers.Storefront;

[ApiController]
[Route("api/v1/publishers")]
public class PublisherController(IPublisherService publisherService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAllAsync(
        [FromQuery] PublisherFilterParam f,
        CancellationToken ct = default
    )
    {
        var result = await publisherService.GetAllAsync(f, ct);
        return Ok(result);
    }
}
