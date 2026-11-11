using Bookstore.Api.DTOs.Authors;
using Bookstore.Api.Services.Storefront;
using Microsoft.AspNetCore.Mvc;

namespace Bookstore.Api.Controllers.Storefront;

[ApiController]
[Route("api/v1/authors")]
public class AuthorController(IAuthorService authorService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAllAuthorsAsync(
        [FromQuery] AuthorFilterParam f,
        CancellationToken ct
    )
    {
        return Ok(await authorService.GetAllAuthorsAsync(f, ct));
    }

    [HttpGet("{authorId:int}")]
    public async Task<IActionResult> GetAuthorByIdAsync(
        [FromRoute] int authorId,
        CancellationToken ct
    )
    {
        var author = await authorService.GetAuthorByIdAsync(authorId, ct);
        if (author is null)
            return NotFound();

        return Ok(author);
    }
}
