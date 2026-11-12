using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi;

namespace Bookstore.Api.Extensions;

public static class OpenApiExtension
{
    public static IServiceCollection AddAppOpenApi(this IServiceCollection services)
    {
        services.AddOpenApi(options =>
        {
            options.AddSchemaTransformer(
                (schema, context, cancellationToken) =>
                {
                    if (context.JsonTypeInfo.Type.IsEnum)
                    {
                        schema.Type = JsonSchemaType.String;
                    }
                    return Task.CompletedTask;
                }
            );

            options.AddDocumentTransformer(
                (document, context, ct) =>
                {
                    document.Components ??= new OpenApiComponents();
                    document.Components.SecuritySchemes ??=
                        new Dictionary<string, IOpenApiSecurityScheme>();
                    document.Components.SecuritySchemes["Bearer"] = new OpenApiSecurityScheme
                    {
                        Type = SecuritySchemeType.Http,
                        Scheme = "bearer",
                        BearerFormat = "JWT",
                    };
                    return Task.CompletedTask;
                }
            );

            options.AddOperationTransformer(
                (operation, context, ct) =>
                {
                    var metadata = context.Description.ActionDescriptor.EndpointMetadata;

                    var authorizeData = metadata.OfType<IAuthorizeData>().ToList();
                    var isAllowAnonymous = metadata.OfType<IAllowAnonymous>().Any();
                    var requresAuth = authorizeData.Count > 0 && !isAllowAnonymous;

                    if (!requresAuth)
                        return Task.CompletedTask;

                    operation.Security ??= [];
                    operation.Security.Add(
                        new OpenApiSecurityRequirement
                        {
                            [new OpenApiSecuritySchemeReference("Bearer", context.Document)] = [],
                        }
                    );

                    var roles = authorizeData
                        .Where(a => !string.IsNullOrWhiteSpace(a.Roles))
                        .SelectMany(a =>
                            a.Roles!.Split(
                                ',',
                                StringSplitOptions.TrimEntries
                                    | StringSplitOptions.RemoveEmptyEntries
                            )
                        )
                        .Distinct()
                        .ToList();

                    var note =
                        roles.Count > 0
                            ? $"Requires authentication with roles: {string.Join(", ", roles)}"
                            : "Requires authentication";

                    operation.Description = string.IsNullOrWhiteSpace(operation.Description)
                        ? note
                        : $"{operation.Description}\n\n{note}";

                    operation.Responses ??= [];
                    operation.Responses.TryAdd(
                        "401",
                        new OpenApiResponse
                        {
                            Description = "Unauthorized (missing or invalid token)",
                        }
                    );
                    if (roles.Count > 0)
                        operation.Responses.TryAdd(
                            "403",
                            new OpenApiResponse
                            {
                                Description = "Forbidden (authenticated but missing required role)",
                            }
                        );

                    return Task.CompletedTask;
                }
            );
        });

        return services;
    }
}
