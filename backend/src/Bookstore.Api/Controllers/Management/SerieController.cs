using Bookstore.Api.Common;
using Bookstore.Api.DTOs.Series;
using Bookstore.Api.Services.Management;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bookstore.Api.Controllers.Management;

[ApiController]
[Route("api/v1/management/series")]
[Authorize(Roles = $"{AppRole.Admin}, {AppRole.Staff}")]
public class SerieController(ISerieManagementService serieManagementService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAllSeries(
        [FromQuery] SerieManagementFilterParam f,
        CancellationToken ct = default
    )
    {
        return Ok(await serieManagementService.GetSeriesAsync(f, ct));
    }

    [HttpPost]
    public async Task<IActionResult> CreateSerie(
        [FromBody] SeriePostDto dto,
        CancellationToken ct = default
    )
    {
        var serie = await serieManagementService.CreateSerieAsync(dto, ct);

        return CreatedAtAction(nameof(GetAllSeries), new { id = serie.Id }, serie);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateSerie(
        int id,
        [FromBody] SeriePutDto dto,
        CancellationToken ct = default
    )
    {
        var serie = await serieManagementService.UpdateSerieAsync(id, dto, ct);

        return Ok(serie);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteSerie(int id, CancellationToken ct = default)
    {
        await serieManagementService.DeleteSerieAsync(id, ct);

        return NoContent();
    }
}
