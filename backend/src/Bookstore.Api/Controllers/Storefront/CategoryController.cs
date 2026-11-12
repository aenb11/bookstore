using Bookstore.Api.DTOs.Categories;
using Bookstore.Api.Services.Storefront;
using Microsoft.AspNetCore.Mvc;

namespace Bookstore.Api.Controllers.Storefront;

[ApiController]
[Route("api/v1/categories")]
public class CategoryController(ICategoryService categoryService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetCategoriesAsync(
        [FromQuery] CategoryFilterParam f,
        CancellationToken ct = default
    )
    {
        var result = await categoryService.GetCategoriesAsync(f, ct);
        return Ok(result);
    }
}
