using System.Linq.Expressions;
using Bookstore.Api.DTOs.Reviews;
using Bookstore.Data.Entities;

namespace Bookstore.Api.Common.Projections;

public static class ReviewProjection
{
    public static Expression<Func<Review, ReviewResponseDto>> ToResponseDto =>
        r => new ReviewResponseDto
        {
            Id = r.Id,
            BookId = r.BookId,
            Rating = r.Rating,
            Comment = r.Comment,
            CreatedAt = r.CreatedAt,
            AccountFullName = r.Account.FullName,
        };

    public static Expression<Func<Review, ReviewDetailResponseDto>> ToDetailResponseDto =>
        r => new ReviewDetailResponseDto
        {
            Id = r.Id,
            BookId = r.BookId,
            AccountId = r.AccountId,
            Rating = r.Rating,
            Comment = r.Comment,
            AccountFullName = r.Account.FullName,
            AccountEmail = r.Account.Email,
            BookTitle = r.Book.Title,
            CreatedAt = r.CreatedAt,
            UpdatedAt = r.UpdatedAt,
            IsDeleted = r.IsDeleted,
            DeletedAt = r.DeletedAt,
        };

    public static readonly Func<Review, ReviewResponseDto> CompileToResponseDto =
        ToResponseDto.Compile();

    public static readonly Func<Review, ReviewDetailResponseDto> CompileToDetailResponseDto =
        ToDetailResponseDto.Compile();
}
