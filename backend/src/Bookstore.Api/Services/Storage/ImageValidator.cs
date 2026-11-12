namespace Bookstore.Api.Services.Storage;

public static class ImageValidator
{
    public static async Task<string?> DetectExtensionAsync(
        Stream stream,
        CancellationToken ct = default
    )
    {
        var header = new byte[12];
        var read = await stream.ReadAsync(header, ct);
        stream.Position = 0;

        if (read >= 3 && header[0] == 0xFF && header[1] == 0xD8 && header[2] == 0xFF)
            return ".jpg";

        if (
            read >= 8
            && header[0] == 0x89
            && header[1] == 0x50
            && header[2] == 0x4E
            && header[3] == 0x47
        )
            return ".png";

        if (
            read >= 12
            && header[0] == 'R'
            && header[1] == 'I'
            && header[2] == 'F'
            && header[3] == 'F'
            && header[8] == 'W'
            && header[9] == 'E'
            && header[10] == 'B'
            && header[11] == 'P'
        )
            return ".webp";

        return null;
    }
}
