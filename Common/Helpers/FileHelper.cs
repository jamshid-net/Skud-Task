using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace Common.Helpers;
public static class FileHelper
{
    public static async Task WriteFileAsync(IFormFile file, string filePath, CancellationToken ct = default)
    {
        if (!Directory.Exists(filePath))
            Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);

        await using var fileStream = new FileStream(filePath,
            FileMode.OpenOrCreate,
            FileAccess.Write,
            FileShare.None,
            bufferSize: 4096,
            useAsync: true);
        await file.CopyToAsync(fileStream, ct);
    }

    public static async Task DeleteFileAsync(string filePath)
    {
        await using (new FileStream(path: filePath,
                             FileMode.Open,
                             FileAccess.Read,
                             FileShare.None,
                             bufferSize: 1,
                             FileOptions.DeleteOnClose | FileOptions.Asynchronous
                         )) { };
    }
    
    public static Image GetThumbnailImage(Image originalImage, Size thumbSize)
    {
        int thWidth = thumbSize.Width;
        int thHeight = thumbSize.Height;

        double ratio = Math.Min((double)thWidth / originalImage.Width, (double)thHeight / originalImage.Height);
        int width = (int)(originalImage.Width * ratio);
        int height = (int)(originalImage.Height * ratio);

        var thumbnailImage = originalImage.Clone(ctx => ctx.Resize(width, height));

        return thumbnailImage;
    }
}
