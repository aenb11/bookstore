using Bookstore.Api.DTOs.Books;
using Bookstore.Api.Services.Storefront;
using Microsoft.AspNetCore.Mvc;

namespace Bookstore.Api.Controllers.Storefront;

[ApiController]
[Route("api/v1/books")]
public class BookController(IBookService bookService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetBookSummariesAsync(
        [FromQuery] BookFilterParam f,
        CancellationToken ct
    )
    {
        return Ok(await bookService.GetBookSummariesAsync(f, ct));
    }

    [HttpGet("{bookId:int}")]
    public async Task<IActionResult> GetBookByIdAsync([FromRoute] int bookId, CancellationToken ct)
    {
        var book = await bookService.GetBookByIdAsync(bookId, ct);

        if (book is null)
            return Problem(statusCode: StatusCodes.Status404NotFound, title: "Book not found");

        return Ok(book);
    }
}
