using Bookstore.Api.Common;
using Bookstore.Api.Common.Pagination;
using Bookstore.Api.DTOs.Books;
using Bookstore.Api.Services.Management;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bookstore.Api.Controllers.Management;

[ApiController]
[Route("api/v1/management/books")]
[Authorize(Roles = $"{AppRole.Admin},{AppRole.Staff}")]
[Tags("Management - Books")]
public class BookController(IBookManagementService bookManagementService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<PagedResult<BookDetailSummaryResponseDto>>> GetBooksAsync(
        [FromQuery] BookManagementFilterParam f,
        CancellationToken ct = default
    )
    {
        var result = await bookManagementService.GetBooksAsync(f, ct);
        return Ok(result);
    }

    [HttpGet("{bookId:int}", Name = "GetManagementBookById")]
    public async Task<ActionResult<BookDetailResponseDto>> GetBookByIdAsync(
        int bookId,
        CancellationToken ct = default
    )
    {
        var book = await bookManagementService.GetBookByIdAsync(bookId, ct);

        if (book is null)
            return Problem(statusCode: StatusCodes.Status404NotFound, title: $"Book not found.");

        return Ok(book);
    }

    [HttpDelete("{bookId:int}")]
    public async Task<IActionResult> DeleteBookAsync(int bookId, CancellationToken ct = default)
    {
        await bookManagementService.DeleteBookAsync(bookId, ct);
        return NoContent();
    }

    [HttpPost]
    public async Task<ActionResult> CreateBookAsync(
        [FromBody] BookPostDto dto,
        CancellationToken ct = default
    )
    {
        var book = await bookManagementService.CreateBookAsync(dto, ct);
        return CreatedAtRoute("GetManagementBookById", new { bookId = book.Id }, book);
    }

    [HttpPut("{bookId:int}")]
    public async Task<ActionResult> UpdateBookAsync(
        [FromRoute] int bookId,
        [FromBody] BookPutDto dto,
        CancellationToken ct = default
    )
    {
        var book = await bookManagementService.UpdateBookAsync(bookId, dto, ct);
        return Ok(book);
    }
}
