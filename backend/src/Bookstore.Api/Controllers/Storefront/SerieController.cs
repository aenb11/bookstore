using Bookstore.Api.DTOs.Series;
using Bookstore.Api.Services.Storefront;
using Microsoft.AspNetCore.Mvc;

namespace Bookstore.Api.Controllers.Storefront;

[ApiController]
[Route("api/v1/series")]
public class SerieController(ISerieService serieService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetSeries([FromQuery] SerieFilterParam f, CancellationToken ct)
    {
        var series = await serieService.GetSeriesAsync(f, ct);
        return Ok(series);
    }
}
