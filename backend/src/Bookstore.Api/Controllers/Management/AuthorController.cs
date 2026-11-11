using Bookstore.Api.Common;
using Bookstore.Api.DTOs.Authors;
using Bookstore.Api.Services.Management;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bookstore.Api.Controllers.Management;

[ApiController]
[Route("api/v1/management/authors")]
[Authorize(Roles = $"{AppRole.Admin},{AppRole.Staff}")]
[Tags("Management - Authors")]
public class AuthorController(IAuthorManagementService authorManagementService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAllAuthorsAsync(
        [FromQuery] AuthorManagementFilterParam f,
        CancellationToken ct
    )
    {
        return Ok(await authorManagementService.GetAllAuthorsAsync(f, ct));
    }

    [HttpGet("{authorId:int}")]
    public async Task<IActionResult> GetAuthorByIdAsync(
        [FromRoute] int authorId,
        CancellationToken ct
    )
    {
        var author = await authorManagementService.GetAuthorByIdAsync(authorId, ct);
        if (author is null)
            return NotFound();

        return Ok(author);
    }

    [HttpPost]
    public async Task<IActionResult> CreateAuthorAsync(
        [FromBody] AuthorPostDto dto,
        CancellationToken ct
    )
    {
        var createdAuthor = await authorManagementService.CreateAuthorAsync(dto, ct);
        return CreatedAtAction(
            nameof(GetAuthorByIdAsync),
            new { authorId = createdAuthor.Id },
            createdAuthor
        );
    }

    [HttpPut("{authorId:int}")]
    public async Task<IActionResult> UpdateAuthorAsync(
        [FromRoute] int authorId,
        [FromBody] AuthorPutDto dto,
        CancellationToken ct
    )
    {
        var updatedAuthor = await authorManagementService.UpdateAuthorAsync(authorId, dto, ct);
        return Ok(updatedAuthor);
    }

    [HttpDelete("{authorId:int}")]
    public async Task<IActionResult> DeleteAuthorAsync(
        [FromRoute] int authorId,
        CancellationToken ct
    )
    {
        await authorManagementService.DeleteAuthorAsync(authorId, ct);
        return NoContent();
    }
}
