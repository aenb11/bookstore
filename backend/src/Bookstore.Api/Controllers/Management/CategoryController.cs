using Bookstore.Api.Common;
using Bookstore.Api.DTOs.Categories;
using Bookstore.Api.Services.Management;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bookstore.Api.Controllers.Management;

[ApiController]
[Route("api/v1/management/categories")]
[Authorize(Roles = $"{AppRole.Admin}, {AppRole.Staff}")]
[Tags("Management - Categories")]
public class CategoryController(ICategoryManagementService categoryManagementService)
    : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAllCategories(
        [FromQuery] CategoryManagementFilterParam f,
        CancellationToken ct = default
    )
    {
        return Ok(await categoryManagementService.GetCategoriesAsync(f, ct));
    }

    [HttpPost]
    public async Task<IActionResult> CreateCategory(
        [FromBody] CategoryPostDto dto,
        CancellationToken ct = default
    )
    {
        var category = await categoryManagementService.CreateCategoryAsync(dto, ct);

        return CreatedAtAction(nameof(GetAllCategories), new { id = category.Id }, category);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateCategory(
        int id,
        [FromBody] CategoryPutDto dto,
        CancellationToken ct = default
    )
    {
        var category = await categoryManagementService.UpdateCategoryAsync(id, dto, ct);

        return Ok(category);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteCategory(int id, CancellationToken ct = default)
    {
        await categoryManagementService.DeleteCategoryAsync(id, ct);

        return NoContent();
    }
}
