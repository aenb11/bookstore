using Bookstore.Api.Common;
using Bookstore.Api.Common.Pagination;
using Bookstore.Api.Common.Projections;
using Bookstore.Api.DTOs.Books;
using Bookstore.Api.Extensions;
using Bookstore.Api.Queries;
using Bookstore.Api.Services.Storage;
using Bookstore.Data;
using Bookstore.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Bookstore.Api.Services.Management;

internal class BookManagementService(AppDbContext ctx, IStorageService storageService)
    : IBookManagementService
{
    public async Task<PagedResult<BookDetailSummaryResponseDto>> GetBooksAsync(
        BookManagementFilterParam f,
        CancellationToken ct = default
    )
    {
        var result = await ctx
            .Books.AsNoTracking()
            .ApplyManagementFilter(f)
            .ApplyManagementSort(f)
            .Select(BookProjection.ToDetailSummaryResponseDto)
            .ToPagedResultAsync(f.PageNumber, f.PageSize, ct);

        foreach (var item in result.Items)
        {
            if (!string.IsNullOrEmpty(item.ThumbnailUrl))
            {
                item.ThumbnailUrl = storageService.GetUrl(item.ThumbnailUrl);
            }
        }

        return result;
    }

    public async Task<BookDetailResponseDto?> GetBookByIdAsync(
        int bookId,
        CancellationToken ct = default
    )
    {
        var book = await ctx
            .Books.AsNoTracking()
            .Where(b => b.Id == bookId)
            .Select(BookProjection.ToDetailResponseDto)
            .FirstOrDefaultAsync(ct);

        if (book is null)
            return null;

        book.Images = [.. book.Images.Select(i => i with { Url = storageService.GetUrl(i.Url) })];

        return book;
    }

    public async Task DeleteBookAsync(int bookId, CancellationToken ct = default)
    {
        var book =
            await ctx.Books.FirstOrDefaultAsync(b => b.Id == bookId, ct)
            ?? throw new InvalidOperationException($"Book with ID {bookId} not found.");

        ctx.Books.Remove(book);
        await ctx.SaveChangesAsync(ct);
    }

    public async Task<BookDetailResponseDto> CreateBookAsync(
        BookPostDto dto,
        CancellationToken ct = default
    )
    {
        var normalizedIsbn = dto.Isbn?.Trim();
        var normalizedTitle = dto.Title.Trim();

        var refCheck =
            await ctx
                .Categories.Where(c => c.Id == dto.CategoryId && !c.IsDeleted)
                .Select(c => new
                {
                    CategoryExists = true,
                    PublisherExists = ctx.Publishers.Any(p =>
                        p.Id == dto.PublisherId && !p.IsDeleted
                    ),
                    SerieValid = !dto.SerieId.HasValue
                        || ctx.Series.Any(s => s.Id == dto.SerieId.Value && !s.IsDeleted),
                    IsbnTaken = !string.IsNullOrEmpty(normalizedIsbn)
                        && ctx.Books.Any(b => b.Isbn == normalizedIsbn && !b.IsDeleted),
                })
                .FirstOrDefaultAsync(ct)
            ?? throw new BadRequestException($"Category with ID {dto.CategoryId} does not exist.");

        if (!refCheck.PublisherExists)
            throw new BadRequestException($"Publisher with ID {dto.PublisherId} does not exist.");

        if (!refCheck.SerieValid)
            throw new BadRequestException($"Serie with ID {dto.SerieId} does not exist.");

        if (refCheck.IsbnTaken)
            throw new ConflictException($"A book with ISBN '{normalizedIsbn}' already exists.");

        var validTags = await ValidateAndFetchTagsAsync(dto.TagIds, ct);

        var authorIds = dto.Authors.Select(a => a.Id).ToList();
        await ValidateAuthorsAsync(authorIds, ct);

        var book = new Book
        {
            Isbn = normalizedIsbn,
            Title = normalizedTitle,
            CategoryId = dto.CategoryId,
            SerieId = dto.SerieId,
            PublisherId = dto.PublisherId,
            PublicationYear = dto.PublicationYear,
            Language = dto.Language.Trim().ToUpperInvariant(),
            PageCount = dto.PageCount,
            CoverType = dto.CoverType,
            Weight = dto.Weight,
            AgeGroup = dto.AgeGroup,
            ListPrice = dto.ListPrice,
            SalePrice = dto.SalePrice,
            StockQuantity = dto.StockQuantity,
            Status = dto.Status,
            Description = dto.Description?.Trim(),

            Images =
            [
                .. dto
                    .Images.OrderBy(i => i.SortOrder)
                    .Select(
                        (img, index) =>
                            new BookImage
                            {
                                BookId = 0,
                                ImageKey = img.ImageKey.Trim(),
                                SortOrder = index,
                            }
                    ),
            ],

            BookAuthors =
            [
                .. dto.Authors.Select(a => new BookAuthor
                {
                    BookId = 0,
                    AuthorId = a.Id,
                    Role = a.Role,
                }),
            ],
        };

        foreach (var tag in validTags)
        {
            book.Tags.Add(tag);
        }

        ctx.Books.Add(book);
        await ctx.SaveChangesAsync(ct);

        return BookProjection.CompiledToDetailResponseDto(book);
    }

    public async Task<BookDetailResponseDto> UpdateBookAsync(
        int bookId,
        BookPutDto dto,
        CancellationToken ct = default
    )
    {
        var normalizedIsbn = dto.Isbn?.Trim();

        var book =
            await ctx
                .Books.AsSplitQuery()
                .Include(b => b.Images)
                .Include(b => b.BookAuthors)
                .Include(b => b.Tags)
                .FirstOrDefaultAsync(b => b.Id == bookId && !b.IsDeleted, ct)
            ?? throw new NotFoundException($"Book with ID {bookId} not found.");

        var refCheck =
            await ctx
                .Categories.Where(c => c.Id == dto.CategoryId && !c.IsDeleted)
                .Select(c => new
                {
                    PublisherExists = ctx.Publishers.Any(p =>
                        p.Id == dto.PublisherId && !p.IsDeleted
                    ),
                    SerieValid = !dto.SerieId.HasValue
                        || ctx.Series.Any(s => s.Id == dto.SerieId.Value && !s.IsDeleted),
                    IsbnTaken = !string.IsNullOrEmpty(normalizedIsbn)
                        && ctx.Books.Any(b =>
                            b.Id != bookId && b.Isbn == normalizedIsbn && !b.IsDeleted
                        ),
                })
                .FirstOrDefaultAsync(ct)
            ?? throw new BadRequestException($"Category with ID {dto.CategoryId} does not exist.");

        if (!refCheck.PublisherExists)
            throw new BadRequestException($"Publisher with ID {dto.PublisherId} does not exist.");

        if (!refCheck.SerieValid)
            throw new BadRequestException($"Serie with ID {dto.SerieId} does not exist.");

        if (refCheck.IsbnTaken)
            throw new ConflictException($"A book with ISBN '{normalizedIsbn}' already exists.");

        var validTags = await ValidateAndFetchTagsAsync(dto.TagIds, ct);
        await ValidateAuthorsAsync([.. dto.Authors.Select(a => a.Id)], ct);

        book.Isbn = normalizedIsbn;
        book.Title = dto.Title.Trim();
        book.CategoryId = dto.CategoryId;
        book.SerieId = dto.SerieId;
        book.PublisherId = dto.PublisherId;
        book.PublicationYear = dto.PublicationYear;
        book.Language = dto.Language.Trim().ToLowerInvariant();
        book.PageCount = dto.PageCount;
        book.CoverType = dto.CoverType;
        book.Weight = dto.Weight;
        book.AgeGroup = dto.AgeGroup;
        book.ListPrice = dto.ListPrice;
        book.SalePrice = dto.SalePrice;
        book.StockQuantity = dto.StockQuantity;
        book.Status = dto.Status;
        book.Description = dto.Description?.Trim();

        SyncTags(book, validTags);
        SyncBookAuthors(book, dto.Authors);
        var removedImageKeys = SyncBookImages(book, dto.Images);

        await ctx.SaveChangesAsync(ct);

        foreach (var imageKey in removedImageKeys)
        {
            await storageService.DeleteAsync(imageKey, ct);
        }

        return BookProjection.CompiledToDetailResponseDto(book);
    }

    private async Task<List<Tag>> ValidateAndFetchTagsAsync(
        IEnumerable<int> tagIds,
        CancellationToken ct
    )
    {
        var distinctIds = tagIds.Distinct().ToList();
        if (distinctIds.Count == 0)
            return [];

        var foundTags = await ctx.Tags.Where(t => distinctIds.Contains(t.Id)).ToListAsync(ct);

        var foundMap = foundTags.ToDictionary(t => t.Id);
        var missingIds = distinctIds.Where(id => !foundMap.ContainsKey(id)).ToList();
        var deletedIds = foundTags.Where(t => t.IsDeleted).Select(t => t.Id).ToList();

        if (missingIds.Count > 0 || deletedIds.Count > 0)
        {
            var details = new List<string>();
            if (missingIds.Count > 0)
                details.Add($"not found: [{string.Join(", ", missingIds)}]");
            if (deletedIds.Count > 0)
                details.Add($"deleted: [{string.Join(", ", deletedIds)}]");
            throw new BadRequestException($"Invalid tag IDs ({string.Join("; ", details)}).");
        }

        return foundTags;
    }

    private async Task ValidateAuthorsAsync(
        IReadOnlyCollection<int> authorIds,
        CancellationToken ct
    )
    {
        var distinctIds = authorIds.Distinct().ToList();

        var foundAuthors = await ctx
            .Authors.AsNoTracking()
            .Where(a => distinctIds.Contains(a.Id))
            .Select(a => new { a.Id, a.IsDeleted })
            .ToListAsync(ct);

        var foundMap = foundAuthors.ToDictionary(a => a.Id);
        var missingIds = distinctIds.Where(id => !foundMap.ContainsKey(id)).ToList();
        var deletedIds = foundAuthors.Where(a => a.IsDeleted).Select(a => a.Id).ToList();

        if (missingIds.Count > 0 || deletedIds.Count > 0)
        {
            var details = new List<string>();
            if (missingIds.Count > 0)
                details.Add($"not found: [{string.Join(", ", missingIds)}]");
            if (deletedIds.Count > 0)
                details.Add($"deleted: [{string.Join(", ", deletedIds)}]");
            throw new BadRequestException($"Invalid author IDs ({string.Join("; ", details)}).");
        }
    }

    private static void SyncTags(Book book, List<Tag> validTags)
    {
        var targetIds = validTags.Select(t => t.Id).ToHashSet();

        var toRemove = book.Tags.Where(t => !targetIds.Contains(t.Id)).ToList();
        foreach (var tag in toRemove)
            book.Tags.Remove(tag);

        var existingIds = book.Tags.Select(t => t.Id).ToHashSet();
        foreach (var tag in validTags.Where(t => !existingIds.Contains(t.Id)))
            book.Tags.Add(tag);
    }

    private static void SyncBookAuthors(
        Book book,
        IReadOnlyCollection<BookPutDto.AuthorItem> incomingAuthors
    )
    {
        var targetIds = incomingAuthors.Select(a => a.Id).ToHashSet();

        var toRemove = book.BookAuthors.Where(ba => !targetIds.Contains(ba.AuthorId)).ToList();
        foreach (var item in toRemove)
            book.BookAuthors.Remove(item);

        var existingMap = book.BookAuthors.ToDictionary(ba => ba.AuthorId);
        foreach (var input in incomingAuthors)
        {
            if (existingMap.TryGetValue(input.Id, out var existing))
            {
                existing.Role = input.Role;
            }
            else
            {
                book.BookAuthors.Add(
                    new BookAuthor
                    {
                        BookId = book.Id,
                        AuthorId = input.Id,
                        Role = input.Role,
                    }
                );
            }
        }
    }

    private static List<string> SyncBookImages(
        Book book,
        IReadOnlyCollection<BookPutDto.BookImageUpdateItem> incomingImages
    )
    {
        var existingMap = book.Images.ToDictionary(img => img.Id);

        foreach (var item in incomingImages.Where(i => i.Id.HasValue))
        {
            if (!existingMap.ContainsKey(item.Id!.Value))
                throw new BadRequestException(
                    $"Image with ID {item.Id.Value} does not belong to this book."
                );
        }

        var keepIds = incomingImages.Where(i => i.Id.HasValue).Select(i => i.Id!.Value).ToHashSet();
        var toRemove = book.Images.Where(img => !keepIds.Contains(img.Id)).ToList();
        var removedKeys = toRemove.Select(img => img.ImageKey).ToList();

        foreach (var img in toRemove)
            book.Images.Remove(img);

        var orderedInputs = incomingImages.OrderBy(i => i.SortOrder).ToList();
        for (var i = 0; i < orderedInputs.Count; i++)
        {
            var input = orderedInputs[i];
            if (input.Id.HasValue)
            {
                var existing = existingMap[input.Id.Value];
                existing.SortOrder = i;
            }
            else
            {
                book.Images.Add(
                    new BookImage
                    {
                        BookId = book.Id,
                        ImageKey = input.ImageKey.Trim(),
                        SortOrder = i,
                    }
                );
            }
        }

        return removedKeys;
    }
}
