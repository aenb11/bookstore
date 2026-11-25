using Bookstore.Api.Common;
using Bookstore.Api.Common.Projections;
using Bookstore.Api.DTOs.Carts;
using Bookstore.Api.Services.Storage;
using Bookstore.Data;
using Bookstore.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Bookstore.Api.Services.Storefront;

internal class CartService(AppDbContext ctx, IStorageService storageService) : ICartService
{
    public const int MaxCartItemsLimit = 20;

    public async Task AddCartItemAsync(
        int accountId,
        CartPostDto dto,
        CancellationToken ct = default
    )
    {
        var book =
            await ctx.Books.AsNoTracking().FirstOrDefaultAsync(b => b.Id == dto.BookId, ct)
            ?? throw new NotFoundException(
                "The book you are trying to add to the cart does not exist."
            );

        if (book.Status != BookStatus.Available)
            throw new BadRequestException(
                "The book you are trying to add to the cart is not available for purchase."
            );

        var existingItem = await ctx.CartItems.FirstOrDefaultAsync(
            ci => ci.AccountId == accountId && ci.BookId == dto.BookId,
            ct
        );

        if (existingItem is not null)
        {
            var newQuantity = existingItem.Quantity + dto.Quantity;
            if (newQuantity > book.StockQuantity)
                throw new BadRequestException(
                    $"The total quantity in the cart exceeds the available stock (only {book.StockQuantity} copies left)."
                );

            existingItem.Quantity = newQuantity;
        }
        else
        {
            if (dto.Quantity > book.StockQuantity)
                throw new BadRequestException(
                    $"The quantity you are trying to add exceeds the available stock (only {book.StockQuantity} copies left)."
                );

            var currentItemCount = await ctx.CartItems.CountAsync(
                ci => ci.AccountId == accountId,
                ct
            );

            if (currentItemCount >= MaxCartItemsLimit)
                throw new BadRequestException(
                    $"You have reached the maximum limit of {MaxCartItemsLimit} items in your cart."
                );

            ctx.CartItems.Add(
                new CartItem
                {
                    AccountId = accountId,
                    BookId = dto.BookId,
                    Quantity = dto.Quantity,
                }
            );
        }

        await ctx.SaveChangesAsync(ct);
    }

    public async Task ClearCartAsync(int accountId, CancellationToken ct = default)
    {
        await ctx.CartItems.Where(ci => ci.AccountId == accountId).ExecuteDeleteAsync(ct);
    }

    public async Task<CartResponseDto> GetCartItemsAsync(
        int accountId,
        CancellationToken ct = default
    )
    {
        var result = await ctx
            .CartItems.AsNoTracking()
            .Where(ci => ci.AccountId == accountId)
            .OrderByDescending(ci => ci.CreatedAt)
            .Select(CartProjection.ToItemResponseDto)
            .ToListAsync(ct);

        var items = result
            .Select(item =>
                item with
                {
                    CoverImageUrl = item.CoverImageUrl is not null
                        ? storageService.GetUrl(item.CoverImageUrl)
                        : null,
                }
            )
            .ToList();

        return new CartResponseDto { Items = items };
    }

    public async Task RemoveCartItemAsync(int accountId, int bookId, CancellationToken ct = default)
    {
        var deletedRows = await ctx
            .CartItems.Where(ci => ci.AccountId == accountId && ci.BookId == bookId)
            .ExecuteDeleteAsync(ct);

        if (deletedRows == 0)
            throw new NotFoundException("The cart item you are trying to remove does not exist.");
    }

    public async Task UpdateCartItemAsync(
        int accountId,
        int bookId,
        CartPutDto dto,
        CancellationToken ct = default
    )
    {
        var existingItem =
            await ctx
                .CartItems.Include(ci => ci.Book)
                .FirstOrDefaultAsync(ci => ci.AccountId == accountId && ci.BookId == bookId, ct)
            ?? throw new NotFoundException(
                "The cart item you are trying to update does not exist."
            );

        if (existingItem.Book is null || existingItem.Book.Status != BookStatus.Available)
            throw new BadRequestException(
                "The book you are trying to update in the cart is not available for purchase."
            );

        if (dto.Quantity > existingItem.Book.StockQuantity)
            throw new BadRequestException(
                $"The quantity you are trying to set exceeds the available stock (only {existingItem.Book.StockQuantity} copies left)."
            );

        existingItem.Quantity = dto.Quantity;
        await ctx.SaveChangesAsync(ct);
    }
}
