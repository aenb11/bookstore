using Bookstore.Api.Services.Storage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Bookstore.Api.Controllers.Common;

[ApiController]
[Route("api/v1/media")]
[Authorize]
public class MediaController(IStorageService storageService) : ControllerBase
{
    [HttpPost("uploads")]
    [Consumes("multipart/form-data")]
    [RequestSizeLimit(5 * 1024 * 1024)]
    public async Task<IActionResult> UploadFile(
        IFormFile file,
        [FromQuery, BindRequired] UploadPurpose purpose,
        CancellationToken ct
    )
    {
        if (!UploadPolicy.All.TryGetValue(purpose, out var policy))
            return Problem(
                statusCode: 400,
                title: "Invalid upload purpose",
                detail: $"Upload purpose '{purpose}' is not supported."
            );

        if (policy.AllowedRoles.Length > 0 && !policy.AllowedRoles.Any(User.IsInRole))
            return Forbid();

        if (file.Length == 0 || file.Length > policy.MaxFileBytes)
            return Problem(
                statusCode: 400,
                title: "Invalid file size",
                detail: $"File size must be between 1 byte and {policy.MaxFileBytes} bytes."
            );

        await using var stream = file.OpenReadStream();

        var ext = await ImageValidator.DetectExtensionAsync(stream, ct);
        if (ext is null || !policy.AllowedExtensions.Contains(ext))
            return Problem(
                statusCode: 400,
                title: "Invalid file type",
                detail: $"File size must be greater than 0 and at most {policy.MaxFileBytes / (1024 * 1024)} MB."
            );

        var key = await storageService.SaveAsync(
            stream,
            $"file{ext}",
            file.ContentType,
            policy.Folder,
            ct
        );

        return Ok(new { key, url = storageService.GetUrl(key) });
    }
}
