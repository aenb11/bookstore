using System.Linq.Expressions;
using Bookstore.Api.DTOs.Addresses;
using Bookstore.Data.Entities;

namespace Bookstore.Api.Common.Projections;

public static class AddressProjection
{
    public static readonly Expression<Func<Address, AddressResponseDto>> ToResponseDto =
        a => new AddressResponseDto
        {
            Id = a.Id,
            IsDefault = a.IsDefault,
            RecipientName = a.RecipientName,
            RecipientPhone = a.RecipientPhone,
            Street = a.Street,
            Ward = a.Ward,
            District = a.District,
            City = a.City,

            CreatedAt = a.CreatedAt,
            UpdatedAt = a.UpdatedAt,
        };

    public static readonly Func<Address, AddressResponseDto> CompileToResponseDto =
        ToResponseDto.Compile();
}
