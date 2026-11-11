using System.Linq.Expressions;
using Bookstore.Api.DTOs.Publishers;
using Bookstore.Data.Entities;

namespace Bookstore.Api.Common.Projections;

public static class PublisherProjection
{
    public static readonly Expression<Func<Publisher, PublisherResponseDto>> ToResponseDto =
        p => new PublisherResponseDto
        {
            Id = p.Id,
            Name = p.Name,
            Website = p.Website,
            ContactEmail = p.ContactEmail,
        };

    public static readonly Expression<
        Func<Publisher, PublisherDetailResponseDto>
    > ToDetailResponseDto = p => new PublisherDetailResponseDto
    {
        Id = p.Id,
        Name = p.Name,
        Website = p.Website,
        ContactEmail = p.ContactEmail,
        CreatedAt = p.CreatedAt,
        UpdatedAt = p.UpdatedAt,
        IsDeleted = p.IsDeleted,
        DeletedAt = p.DeletedAt,
    };
}
